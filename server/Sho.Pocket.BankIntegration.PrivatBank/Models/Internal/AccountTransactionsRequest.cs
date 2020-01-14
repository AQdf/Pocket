using System;
using Sho.BankIntegration.Privatbank.Utils;

namespace Sho.BankIntegration.Privatbank.Models.Internal
{
    internal class AccountTransactionsRequest
    {
        public AccountTransactionsRequest(string password, string merchantId, string cardNumber, DateTime from, DateTime to)
        {
            Password = password;
            CardNumber = cardNumber;
            MerchantId = merchantId;
            From = from.ToDotFormatString();
            To = to.ToDotFormatString();
            Xml = Compose();
        }

        /// <summary>
        /// Private merchant password.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Merchant assossiated card number.
        /// </summary>
        public string CardNumber { get; }

        /// <summary>
        /// Merchant id.
        /// </summary>
        public string MerchantId { get; }

        /// <summary>
        /// Start date of the statement period in format 'dd.mm.yyyy'.
        /// </summary>
        public string From { get; }

        /// <summary>
        /// End date of the statement period in format 'dd.mm.yyyy'.
        /// </summary>
        public string To { get; }

        /// <summary>
        /// Composed merchant balance request xml.
        /// </summary>
        public string Xml { get; }

        /// <summary>
        /// Make sure that the contents of dataContent and the corresponding substring in xml match up to a byte.
        /// </summary>
        private string Compose()
        {
            string dataTagContent = $"<oper>cmt</oper><wait>0</wait><test>0</test><payment><prop name=\"sd\" value=\"{From}\" /><prop name=\"ed\" value=\"{To}\" /><prop name=\"card\" value=\"{CardNumber}\" /></payment>";
            string plainSignature = $"{dataTagContent}{Password}";
            string signature = plainSignature.GetHashMd5().GetHashSha1();
            string text = $"<?xml version=\"1.0\" encoding=\"UTF-8\"?><request version=\"1.0\"><merchant><id>{MerchantId}</id><signature>{signature}</signature></merchant><data>{dataTagContent}</data></request>";

            return text;
        }
    }
}
