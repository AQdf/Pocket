using Newtonsoft.Json;
using Sho.BankIntegration.Monobank.Models;
using Sho.BankIntegration.Monobank.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sho.BankIntegration.Monobank.Services
{
    public class MonobankAccountService
    {
        /// <summary>
        /// Gets client accounts. Monobank API reference: <https://api.monobank.ua/docs/#tag---------------------------->
        /// </summary>
        /// <param name="token">Client auth token</param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<MonobankAccount>> GetClientAccountsAsync(string token)
        {
            string requestUri = MonobankConfiguration.BANK_API_URL + "personal/client-info";
            IReadOnlyCollection<MonobankAccount> accounts;

            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
                request.Headers.Add("X-Token", token);

                HttpResponseMessage response = await client.SendAsync(request);
                string content = await response.Content.ReadAsStringAsync();
                MonobankClientInfo clientInfo = JsonConvert.DeserializeObject<MonobankClientInfo>(content);

                accounts = clientInfo.Accounts
                    .Select(a => new MonobankAccount(a.Id, a.Balance, a.CreditLimit, ISO4217CurrencyConverter.GetCurrencyName(a.CurrencyCode), a.CashbackType))
                    .ToList();
            }

            return accounts;
        }

        /// <summary>
        /// Gets client account extract. Monobank API reference: <https://api.monobank.ua/docs/#tag---------------------------->
        /// </summary>
        /// <param name="token">Client auth token</param>
        /// <returns></returns>
        public Task<string> GetClientAccountExctractAsync(string token)
        {
            throw new NotImplementedException();
        }
    }
}
