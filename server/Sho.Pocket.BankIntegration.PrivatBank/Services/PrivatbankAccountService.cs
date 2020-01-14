using System;
using System.Net.Http;
using System.Threading.Tasks;
using Sho.BankIntegration.Privatbank.Models;
using Sho.BankIntegration.Privatbank.Models.Internal;
using Sho.BankIntegration.Privatbank.Parsers;

namespace Sho.BankIntegration.Privatbank.Services
{
    public class PrivatbankAccountService
    {
        private readonly PrivatbankClient _privatbankClient;

        public PrivatbankAccountService(PrivatbankClient privatbankClient)
        {
            _privatbankClient = privatbankClient;
        }

        /// <summary>
        /// Gets merchant account. Privatbank API reference: <https://api.privatbank.ua/#p24/balance>.
        /// </summary>
        /// <param name="password">Private merchant password.</param>
        /// <param name="merchantId">Merchant id.</param>
        /// <param name="cardNumber">Merchant assossiated card number.</param>
        /// <returns></returns>
        public async Task<PrivatbankAccount> GetMerchantAccountAsync(string password, string merchantId, string cardNumber)
        {
            AccountBalanceRequest request = new AccountBalanceRequest(password, merchantId, cardNumber);
            HttpResponseMessage response = await _privatbankClient.GetMerchantDataAsync("balance", request.Xml);
            string xml = await response.Content.ReadAsStringAsync();

            AccountParser parser = new AccountParser();
            PrivatbankAccount account = parser.Parse(xml);

            return account;
        }

        /// <summary>
        /// Gets merchant account transactions.
        /// Privatbank API reference: <https://api.privatbank.ua/#p24/orders>.
        /// </summary>
        /// <param name="password">Private merchant password.</param>
        /// <param name="merchantId">Merchant id.</param>
        /// <param name="cardNumber">Merchant assossiated card number.</param>
        /// <param name="from">Statement start date.</param>
        /// <param name="to">Statement end date.</param>
        /// <returns></returns>
        public async Task<PrivatbankAccountStatement> GetMerchantAccountStatementAsync(
            string password, string merchantId, string cardNumber, DateTime from, DateTime to)
        {
            AccountTransactionsRequest request = new AccountTransactionsRequest(password, merchantId, cardNumber, from, to);
            HttpResponseMessage response = await _privatbankClient.GetMerchantDataAsync("rest_fiz", request.Xml);
            string xml = await response.Content.ReadAsStringAsync();

            AccountStatementParser parser = new AccountStatementParser();
            PrivatbankAccountStatement statement = parser.Parse(xml);

            return statement;
        }
    }
}
