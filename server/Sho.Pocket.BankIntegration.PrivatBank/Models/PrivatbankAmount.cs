namespace Sho.BankIntegration.Privatbank.Models
{
    /// <summary>
    /// Privatbank amount.
    /// </summary>
    public class PrivatbankAmount
    {
        public PrivatbankAmount(string currency, decimal value)
        {
            Currency = currency;
            Value = value;
        }

        /// <summary>
        /// Currency name according to ISO4217.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Amount value.
        /// </summary>
        public decimal Value { get; set; }
    }
}
