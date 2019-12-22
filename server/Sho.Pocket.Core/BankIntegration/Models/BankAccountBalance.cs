namespace Sho.Pocket.Core.BankIntegration.Models
{
    public class BankAccountBalance
    {
        public BankAccountBalance(string bankName, string accountId, string accountName, string currency, decimal balance)
        {
            BankName = bankName;
            AccountId = accountId;
            AccountName = accountName;
            Currency = currency;
            Balance = balance;
        }

        public string BankName { get; set; }

        public string AccountId { get; set; }

        public string AccountName { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }
    }
}
