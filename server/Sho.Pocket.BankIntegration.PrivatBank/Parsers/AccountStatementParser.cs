using System;
using System.Collections.Generic;
using System.Xml;
using Sho.BankIntegration.Privatbank.Models;
using Sho.BankIntegration.Privatbank.Parsers.Abstractions;
using Sho.BankIntegration.Privatbank.Utils;

namespace Sho.BankIntegration.Privatbank.Parsers
{
    internal class AccountStatementParser : PrivatbankXmlResponseParser<PrivatbankAccountStatement>
    {
        private readonly AmountParser _amountParser = new AmountParser();

        public override PrivatbankAccountStatement Parse(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            XmlNode statementsNode = document.SelectSingleNode("response/data/info/statements");

            string status = GetAttributeText(statementsNode, "status");
            string creditString = GetAttributeText(statementsNode, "credit");
            string debetString = GetAttributeText(statementsNode, "debet");
            XmlNodeList statements = statementsNode.ChildNodes;

            decimal? credit = NullableDecimalConverter.Convert(creditString);
            decimal? debet = NullableDecimalConverter.Convert(debetString);
            List<PrivatbankAccountTransaction> items = ParseItems(statements);

            return new PrivatbankAccountStatement(status, credit, debet, items);
        }

        private List<PrivatbankAccountTransaction> ParseItems(XmlNodeList statements)
        {
            List<PrivatbankAccountTransaction> items = new List<PrivatbankAccountTransaction>();

            if (statements == null)
            {
                return items;
            }

            for (int i = 0; i < statements.Count; i++)
            {
                XmlNode node = statements.Item(i);
                string card = GetAttributeText(node, "card");
                string appcode = GetAttributeText(node, "appcode");
                string trandate = GetAttributeText(node, "trandate");
                string trantime = GetAttributeText(node, "trantime");
                string tranamountText = GetAttributeText(node, "amount");
                string cardamountText = GetAttributeText(node, "cardamount");
                string rest = GetAttributeText(node, "rest");
                string terminal = GetAttributeText(node, "terminal");
                string description = GetAttributeText(node, "description");

                DateTime date = DateTime.Parse($"{trandate} {trantime}");
                PrivatbankAmount cardAmount = _amountParser.Parse(cardamountText);
                PrivatbankAmount tranAmount = _amountParser.Parse(tranamountText);
                PrivatbankAmount balance = _amountParser.Parse(rest);

                items.Add(new PrivatbankAccountTransaction(
                    card, appcode, date, cardAmount, tranAmount, balance, description, terminal));
            }

            return items;
        }
    }
}
