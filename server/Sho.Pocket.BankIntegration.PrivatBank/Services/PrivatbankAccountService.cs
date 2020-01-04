using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            string requestUri = $"{PrivatbankConfiguration.BANK_API_URL}/balance";
            string xml;

            using (HttpClient client = new HttpClient())
            {
                AccountBalanceRequest request = new AccountBalanceRequest(password, merchantId, cardNumber);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, requestUri)
                {
                    Content = new StringContent(request.Xml, Encoding.UTF8, "text/xml")
                };

                HttpResponseMessage response = await client.SendAsync(message);
                xml = await response.Content.ReadAsStringAsync();
            }

            AccountBalanceResponse balance = AccountBalanceResponse.Parse(xml);

            if (string.IsNullOrWhiteSpace(balance.Id) || string.IsNullOrWhiteSpace(balance.Currency) || !balance.Balance.HasValue)
            {
                throw new Exception("Failed to parse merchant account data");
            }

            PrivatbankAccount account = new PrivatbankAccount(
                balance.Id,
                balance.Currency,
                balance.Balance.Value,
                balance.CreditLimit ?? decimal.Zero,
                balance.Name);

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
        public async Task<IReadOnlyCollection<PrivatbankAccountTransaction>> GetMerchantAccountTransactionsAsync(
            string password, string merchantId, string cardNumber, DateTime from, DateTime to)
        {
            string requestUri = $"{PrivatbankConfiguration.BANK_API_URL}/rest_fiz";
            string xml;

            using (HttpClient client = new HttpClient())
            {
                AccountTransactionsRequest request = new AccountTransactionsRequest(password, merchantId, cardNumber, from , to);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, requestUri)
                {
                    Content = new StringContent(request.Xml, Encoding.UTF8, "text/xml")
                };

                HttpResponseMessage response = await client.SendAsync(message);
                xml = await response.Content.ReadAsStringAsync();
            }

            AccountStatementResponse statement = AccountStatementResponse.Parse(xml);

            IReadOnlyCollection<PrivatbankAccountTransaction> transactions = statement.Items
                .Select(i => ParseTransaction(i))
                .Where(t => t != null)
                .ToList();

            return transactions;
        }

        private PrivatbankAccountTransaction ParseTransaction(AccountStatementItemResponse item)
        {
            DateTime transactionDate = DateTime.Parse($"{item.TransactionDate} {item.TransactionTime}");
            PrivatbankTransactionAmount amount = ParseAmount(item.CardAmount);
            PrivatbankTransactionAmount balance = ParseAmount(item.Rest);

            if (amount != null)
            {
                return new PrivatbankAccountTransaction(item.Card, item.AppCode, transactionDate, amount.Value, amount.Currency.Name, balance.Value, item.Description);
            }

            return null;
        }

        /// <summary>
        /// Parses amount string returned by Privatbank API.
        /// </summary>
        /// <param name="amount">Amount string in format '-0.10 UAH'</param>
        /// <returns></returns>
        private PrivatbankTransactionAmount ParseAmount(string amount)
        {
            string[] amountParts = amount.Split(' ');

            if (amountParts.Length != 2)
            {
                throw new Exception("Failed to parse Privatbank amount currency.");
            }

            bool valueParsed = decimal.TryParse(amountParts[0], out decimal value);

            if (!valueParsed)
            {
                throw new Exception("Failed to parse Privatbank amount value.");
            }

            return new PrivatbankTransactionAmount(value, PrivatbankCurrency.GetCompatible(amountParts[1]));
        }
    }
}
