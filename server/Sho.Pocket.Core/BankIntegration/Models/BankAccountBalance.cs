namespace Sho.Pocket.Core.BankIntegration.Models
{
    public class BankAccountBalance
    {
        public string AccountId { get; set; }

        public string AccountName { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }
    }
}
