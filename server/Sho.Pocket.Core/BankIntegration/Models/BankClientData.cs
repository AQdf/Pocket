namespace Sho.Pocket.Core.BankIntegration.Models
{
    public class BankClientData
    {
        public BankClientData(string token, string bankClientId, string cardNumber)
        {
            Token = token;
            BankClientId = bankClientId;
            CardNumber = cardNumber;
        }

        public string Token { get; set; }

        public string BankClientId { get; set; }

        public string CardNumber { get; set; }
    }
}
