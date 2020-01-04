namespace Sho.BankIntegration.Privatbank.Models
{
    internal class PrivatbankTransactionAmount
    {
        public PrivatbankTransactionAmount(decimal value, PrivatbankCurrency currency)
        {
            Value = value;
            Currency = currency;
        }

        public decimal Value { get; set; }

        public PrivatbankCurrency Currency { get; set; }
    }
}
