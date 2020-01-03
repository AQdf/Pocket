namespace Sho.Pocket.Core.Features.BankSync.Models
{
    public class BankAccountsRequestParams
    {
        public BankAccountsRequestParams(string token)
        {
            Token = token;
        }

        public BankAccountsRequestParams(string token, string id, string cardNumber)
        {
            Token = token;
            Id = id;
            CardNumber = cardNumber;
        }

        public string Token { get; }

        public string Id { get; }

        public string CardNumber { get; }
    }
}
