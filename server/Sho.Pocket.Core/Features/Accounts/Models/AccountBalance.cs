using Sho.Pocket.Core.BankIntegration.Models;

namespace Sho.Pocket.Core.Features.Accounts.Models
{
    public class AccountBalance
    {
        public AccountBalance(BankAccountBalance a)
        {
            AccountName = a.AccountName;
            Currency = a.Currency;
            Balance = a.Balance;
        }

        public string AccountName { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }
    }
}
