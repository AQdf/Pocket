using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sho.BankIntegration.Monobank.Models;
using Sho.BankIntegration.Monobank.Utils;

namespace Sho.BankIntegration.Monobank.Services
{
    public class MonobankAccountService
    {
        private readonly MonobankClient _monobankClient;

        public MonobankAccountService(MonobankClient monobankClient)
        {
            _monobankClient = monobankClient;
        }

        /// <summary>
        /// Gets client accounts.
        /// Monobank API reference: <https://api.monobank.ua/docs/#tag---------------------------->.
        /// </summary>
        /// <param name="token">Client auth token.</param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<MonobankAccount>> GetAccountsAsync(string token)
        {
            string json = await _monobankClient.GetPersonalDataAsync("personal/client-info", token);
            MonobankClientInfo clientInfo = JsonConvert.DeserializeObject<MonobankClientInfo>(json);

            IReadOnlyCollection<MonobankAccount> accounts = clientInfo.Accounts
                .Select(a => ParseAccount(a))
                .Where(account => account != null)
                .ToList();

            return accounts;
        }

        /// <summary>
        /// Gets client account statement for the specified period.
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
            string json = await _monobankClient.GetPersonalDataAsync($"personal/statement/{account}/{fromTime}/{toTime}", token);

            IEnumerable<MonobankStatementItem> items = JsonConvert.DeserializeObject<IEnumerable<MonobankStatementItem>>(json);

            IReadOnlyCollection<MonobankAccountTransaction> transactions = items
                .Select(i => ParseTransaction(i))
                .Where(t => t != null)
                .ToList();

            return transactions;
        }

        private MonobankAccount ParseAccount(MonobankClientAccount item)
        {
            bool currencyParsed = MonobankCurrency.TryParse(item.CurrencyCode, out MonobankCurrency currency);

            if (currencyParsed)
            {
                return new MonobankAccount(item.Id, item.Balance, item.CreditLimit, currency.Name, item.CashbackType);
            }
            else
            {
                //TODO: Log warning: Monobank account parsing failed!
                return null;
            }
        }

        private MonobankAccountTransaction ParseTransaction(MonobankStatementItem item)
        {
            bool currencyParsed = MonobankCurrency.TryParse(item.CurrencyCode, out MonobankCurrency currency);

            if (currencyParsed)
            {
                return new MonobankAccountTransaction(item.Id, item.Time.FromUnixTime(), item.Description, currency.Name, item.Amount, item.Balance);
            }
            else
            {
                //TODO: Log warning: Monobank transaction parsing failed!
                return null;
            }
        }
    }
}
