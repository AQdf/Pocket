using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sho.BankIntegration.Monobank.Models;

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
            string requestUri = $"{MonobankConfiguration.BANK_API_URL}/personal/client-info";
            string json;

            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
                request.Headers.Add("X-Token", token);
                HttpResponseMessage response = await client.SendAsync(request);
                json = await response.Content.ReadAsStringAsync();
            }

            MonobankClientInfo clientInfo = JsonConvert.DeserializeObject<MonobankClientInfo>(json);

            IReadOnlyCollection<MonobankAccount> accounts = clientInfo.Accounts
                .Select(a => ParseAccount(a))
                .Where(account => account != null)
                .ToList();

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
    }
}
