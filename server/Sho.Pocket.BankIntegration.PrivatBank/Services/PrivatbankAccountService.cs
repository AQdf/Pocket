using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System;
using Sho.BankIntegration.Privatbank.Models;

namespace Sho.BankIntegration.Privatbank.Services
{
    public class PrivatbankAccountService
    {
        /// <summary>
        /// Gets merchant account. Privatbank API reference: <https://api.privatbank.ua/#p24/balance>.
        /// </summary>
        /// <param name="password">Private merchant password.</param>
        /// <param name="merchantId">Merchant id.</param>
        /// <param name="cardNumber">Merchant assossiated card number.</param>
        /// <returns></returns>
        public async Task<PrivatbankAccount> GetMerchantAccountAsync(string password, string merchantId, string cardNumber)
        {
            string requestUri = PrivatbankConfiguration.BANK_API_URL + "balance";
            PrivatbankAccount account;

            using (HttpClient client = new HttpClient())
            {
                AccountBalanceRequest request = new AccountBalanceRequest(password, merchantId, cardNumber);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, requestUri)
                {
                    Content = new StringContent(request.Xml, Encoding.UTF8, "text/xml")
                };

                HttpResponseMessage response = await client.SendAsync(message);
                string content = await response.Content.ReadAsStringAsync();
                AccountBalanceResponse balance = AccountBalanceResponse.Parse(content);

                if (string.IsNullOrWhiteSpace(balance.Id) || string.IsNullOrWhiteSpace(balance.Currency) || !balance.Balance.HasValue)
                {
                    throw new Exception("Failed to parse merchant account data");
                }

                account = new PrivatbankAccount(balance.Id,balance.Currency, balance.Balance.Value, balance.CreditLimit ?? decimal.Zero, balance.Name);
            }

            return account;
        }

        /// <summary>
        /// Gets merchant account extract. Privatbank API reference: <https://api.privatbank.ua/#p24/orders>.
        /// </summary>
        /// <param name="password">Private merchant password.</param>
        /// <param name="merchantId">Merchant id.</param>
        /// <param name="cardNumber">Merchant assossiated card number.</param>
        /// <returns></returns>
        public Task<string> GetClientAccountExctractAsync(string password, string merchantId, string cardNumber)
        {
            throw new NotImplementedException();
        }
    }
}
