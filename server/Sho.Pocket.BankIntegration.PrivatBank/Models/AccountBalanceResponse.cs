using System.Xml;

namespace Sho.BankIntegration.Privatbank.Models
{
    /// <summary>
    /// Example: <https://api.privatbank.ua/#p24/balance>.
    /// </summary>
    internal class AccountBalanceResponse
    {
        private AccountBalanceResponse(string id, string currency, decimal? balance, decimal? creditLimit, string name)
        {
            Id = id;
            Currency = currency;
            Balance = balance;
            CreditLimit = creditLimit;
            Name = name;
        }

        /// <summary>
        /// Card number
        /// </summary>
        public string Id { get; }

        public string Currency { get; }

        public decimal? Balance { get; }

        public decimal? CreditLimit { get; }

        public string Name { get; }

        public static AccountBalanceResponse Parse(string content)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(content);

            string id = document.SelectSingleNode("response/data/info/cardbalance/card/card_number").InnerText;
            string currency = document.SelectSingleNode("response/data/info/cardbalance/card/currency").InnerText;
            string name = document.SelectSingleNode("response/data/info/cardbalance/card/acc_name").InnerText;
            string balanceString = document.SelectSingleNode("response/data/info/cardbalance/balance").InnerText;
            string creditLimitString = document.SelectSingleNode("response/data/info/cardbalance/fin_limit").InnerText;

            decimal? balance = null;
            bool balanceParsed = decimal.TryParse(balanceString, out decimal balanceValue);

            if (balanceParsed)
            {
                balance = balanceValue;
            }

            decimal? creditLimit = null;
            bool creditLimitParsed = decimal.TryParse(creditLimitString, out decimal creditLimitValue);

            if (creditLimitParsed)
            {
                creditLimit = creditLimitValue;
            }

            return new AccountBalanceResponse(id, currency, balance, creditLimit, name);
        }
    }
}
