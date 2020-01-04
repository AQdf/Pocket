namespace Sho.Pocket.Core.Features.BankSync.Models
{
    public class BankAccountsRequestParams
    {
        public BankAccountsRequestParams(string token)
        {
            Token = token;
        }

        public BankAccountsRequestParams(string token, string bankClientId, string cardNumber)
        {
            Token = token;
            BankClientId = bankClientId;
            CardNumber = cardNumber;
        }

        public string Token { get; }

        public string BankClientId { get; }

        public string CardNumber { get; }
    }
}
