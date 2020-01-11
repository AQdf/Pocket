using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sho.BankIntegration.Monobank.Models;
using Sho.BankIntegration.Monobank.Models.Internal;
using Sho.BankIntegration.Monobank.Utils;

namespace Sho.BankIntegration.Monobank.Services
{
    public class MonobankAccountService
    {
        private readonly MonobankHttpClient _monobankClient;

        public MonobankAccountService(MonobankHttpClient monobankClient)
        {
            _monobankClient = monobankClient;
        }

        /// <summary>
        /// Gets information about client and client accounts.
        /// Monobank API reference: <https://api.monobank.ua/docs/#tag---------------------------->.
        /// </summary>
        /// <param name="token">Client auth token.</param>
        /// <returns></returns>
        public async Task<MonobankClientInfo> GetClientInfoAsync(string token)
        {
            HttpResponseMessage response = await _monobankClient.GetPersonalDataAsync("personal/client-info", token);
            string json = await response.Content.ReadAsStringAsync();
            ClientInfo info = JsonConvert.DeserializeObject<ClientInfo>(json);

            var accounts = info.Accounts
                .Select(a => new MonobankAccount(a.Id, a.Balance, a.CreditLimit, a.CurrencyCode, a.CashbackType))
                .ToList();

            return new MonobankClientInfo(info.Name, info.WebHookUrl, accounts);
        }

        /// <summary>
        /// Gets client account transactions for the specified period.
        /// Maximum statement interval is 31 days + 1 hour (2682000 seconds).
        /// Maximum request interval is 1 time in 60 seconds.
        /// Monobank API reference: <https://api.monobank.ua/docs/#operation--personal-statement--account---from---to--get>.
        /// </summary>
        /// <param name="token">Client auth token.</param>
        /// <param name="account">Account identifier from statement list or 0 for default account.</param>
        /// <param name="from">Start time of the statement.</param>
        /// <param name="to">End time of the statement.</param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<MonobankAccountTransaction>> GetAccountTransactionsAsync(string token, string account, DateTime from, DateTime to)
        {
            string fromTime = from.ToUnixTime().ToString();
            string toTime = to.ToUnixTime().ToString();
            HttpResponseMessage response = await _monobankClient.GetPersonalDataAsync($"personal/statement/{account}/{fromTime}/{toTime}", token);
            string json = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<IEnumerable<StatementItem>>(json);

            var transactions = items
                .Select(t => new MonobankAccountTransaction(t.Id, t.Time.FromUnixTime(), t.Description, t.CurrencyCode, t.Amount, t.Balance))
                .ToList();

            return transactions;
        }

        /// <summary>
        /// Sets service URL to receive POST responses about client new transactions.
        /// If the service URL will not respond in 5 seconds, the request will be retried in 60 and 600 seconds.
        /// After retries without a success webhook functionality will be disabled.
        /// Monobank API reference: <https://api.monobank.ua/docs/#operation--personal-webhook-post>.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="webHookUrl"></param>
        /// <returns></returns>
        public async Task<bool> SetWebHookAsync(string token, string webHookUrl)
        {
            string body = JsonConvert.SerializeObject(new { webHookUrl = webHookUrl });
            HttpResponseMessage response = await _monobankClient.PostAsync("personal/webhook", body, token);

            return response.IsSuccessStatusCode;
        }
    }
}
