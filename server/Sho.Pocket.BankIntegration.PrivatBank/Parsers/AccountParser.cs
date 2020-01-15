using System.Xml;
using Sho.BankIntegration.Privatbank.Models;
using Sho.BankIntegration.Privatbank.Parsers.Abstractions;

namespace Sho.BankIntegration.Privatbank.Parsers
{
    internal class AccountParser : PrivatbankXmlResponseParser<PrivatbankAccount>
    {
        public override PrivatbankAccount Parse(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            XmlNode cardBalanceNode = document.SelectSingleNode("response/data/info/cardbalance");

            string id = GetNodeText(cardBalanceNode, "card/card_number");
            string name = GetNodeText(cardBalanceNode, "card/acc_name");
            string currency = GetNodeText(cardBalanceNode, "card/currency");
            string balanceText = GetNodeText(cardBalanceNode, "balance");
            string creditLimitText = GetNodeText(cardBalanceNode, "fin_limit");

            decimal balance  = decimal.Parse(balanceText);
            decimal creditLimit = decimal.Parse(creditLimitText);

            return new PrivatbankAccount(id, name, currency, balance, creditLimit);
        }
    }
}
