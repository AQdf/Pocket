using System.Collections.Generic;
using System.Xml;

namespace Sho.BankIntegration.Privatbank.Models
{
    /// <summary>
    /// Example: <https://api.privatbank.ua/#p24/orders>.
    /// </summary>
    internal class AccountStatementResponse
    {
        private AccountStatementResponse(string status, decimal? credit, decimal? debet, IReadOnlyCollection<AccountStatementItemResponse> items)
        {
            Status = status;
            Credit = credit;
            Debet = debet;
            Items = items;
        }

        /// <summary>
        /// Possible values: [excellent].
        /// </summary>
        public string Status { get; }

        /// <summary>
        /// Total amount of expenses.
        /// </summary>
        public decimal? Credit { get; }

        /// <summary>
        /// Total amount of income.
        /// </summary>
        public decimal? Debet { get; }

        public IReadOnlyCollection<AccountStatementItemResponse> Items { get; }

        public static AccountStatementResponse Parse(string content)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(content);

            string status = document.SelectSingleNode("response/data/info/statements").Attributes.GetNamedItem("status").InnerText;
            string creditString = document.SelectSingleNode("response/data/info/statements").Attributes.GetNamedItem("credit").InnerText;
            string debetString = document.SelectSingleNode("response/data/info/statements").Attributes.GetNamedItem("debet").InnerText;

            decimal? credit = null;
            bool creditParsed = decimal.TryParse(creditString, out decimal creditValue);

            if (creditParsed)
            {
                credit = creditValue;
            }

            decimal? debet = null;
            bool debetParsed = decimal.TryParse(debetString, out decimal debetValue);

            if (debetParsed)
            {
                debet = debetValue;
            }

            XmlNodeList statements = document.SelectSingleNode("response/data/info/statements").ChildNodes;
            List<AccountStatementItemResponse> items = ParseItems(statements);

            return new AccountStatementResponse(status, credit, debet, items);
        }

        private static List<AccountStatementItemResponse> ParseItems(XmlNodeList statements)
        {
            List<AccountStatementItemResponse> items = new List<AccountStatementItemResponse>();

            for (int i = 0; i < statements.Count; i++)
            {
                XmlNode node = statements.Item(i);
                string card = node.Attributes.GetNamedItem("card").InnerText;
                string appcode = node.Attributes.GetNamedItem("appcode").InnerText;
                string trandate = node.Attributes.GetNamedItem("trandate").InnerText;
                string trantime = node.Attributes.GetNamedItem("trantime").InnerText;
                string amount = node.Attributes.GetNamedItem("amount").InnerText;
                string cardamount = node.Attributes.GetNamedItem("cardamount").InnerText;
                string rest = node.Attributes.GetNamedItem("rest").InnerText;
                string terminal = node.Attributes.GetNamedItem("terminal").InnerText;
                string description = node.Attributes.GetNamedItem("description").InnerText;

                items.Add(new AccountStatementItemResponse(card, appcode, trandate, trantime, amount, cardamount, rest, terminal, description));
            }

            return items;
        }
    }
}
