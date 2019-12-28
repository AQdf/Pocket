namespace Sho.Pocket.Core.Features.BankSync.Models
{
    public class BankClientData
    {
        public BankClientData(string token)
        {
            Token = token;
        }

        public BankClientData(string token, string id, string cardNumber)
        {
            Token = token;
            Id = id;
            CardNumber = cardNumber;
        }

        public string Token { get; set; }

        public string Id { get; set; }

        public string CardNumber { get; set; }
    }
}
