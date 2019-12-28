using Sho.BankIntegration.Privatbank.Utils;

namespace Sho.BankIntegration.Privatbank.Models
{
    /// <summary>
    /// Example: <https://api.privatbank.ua/#p24/balance>.
    /// </summary>
    /// <param name="password">Private merchant password.</param>
    /// <param name="merchantId">Merchant id.</param>
    /// <param name="cardNumber">Merchant assossiated card number.</param>
    /// <returns></returns>
    internal class AccountBalanceRequest
    {
        public AccountBalanceRequest(string password, string merchantId, string cardNumber)
        {
            Password = password;
            CardNumber = cardNumber;
            MerchantId = merchantId;
            Xml = Compose(password, cardNumber, merchantId);
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
        /// Composed merchant balance request xml.
        /// </summary>
        public string Xml { get; }

        /// <summary>
        /// Make sure that the contents of dataContent and the corresponding substring in xml match up to a byte.
        /// </summary>
        private string Compose(string password, string cardNumber, string merchantId)
        {
            string dataTagContent = $"<oper>cmt</oper><wait>0</wait><test>0</test><payment id=\"\"><prop name=\"cardnum\" value=\"{cardNumber}\" /><prop name=\"country\" value=\"UA\" /></payment>";
            string plainSignature = $"{dataTagContent}{password}";
            string signature = plainSignature.GetHashMd5().GetHashSha1();
            string text = $"<?xml version=\"1.0\" encoding=\"UTF-8\"?><request version=\"1.0\"><merchant><id>{merchantId}</id><signature>{signature}</signature></merchant><data>{dataTagContent}</data></request>";

            return text;
        }
    }
}
