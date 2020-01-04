using System;

namespace Sho.Pocket.Core.Features.BankSync.Models
{
    public class AccountStatementRequestParams
    {
        public AccountStatementRequestParams(string token, string account, DateTime from, DateTime to)
        {
            Token = token;
            Account = account;
            From = from;
            To = to;
        }

        public AccountStatementRequestParams(string token, string account, string bankClientId, DateTime from, DateTime to)
        {
            Token = token;
            Account = account;
            BankClientId = bankClientId;
            From = from;
            To = to;
        }

        public string Token { get; }

        public string Account { get; }

        public string BankClientId { get; set; }

        public DateTime From { get; }

        public DateTime To { get; }
    }
}
