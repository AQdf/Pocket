namespace Sho.BankIntegration.Privatbank.Models
{
    /// <summary>
    /// Privatbank account information
    /// </summary>
    public class PrivatbankAccount
    {
        public PrivatbankAccount(string id, string currency, decimal balance, decimal creditLimit, string name)
        {
            Id = id;
            Currency = currency;
            Balance = balance;
            CreditLimit = creditLimit;
            Name = !string.IsNullOrWhiteSpace(name) 
                ? name 
                : $"{PrivatbankDefaultConfig.BANK_NAME}: {balance} {currency}";
        }

        /// <summary>
        /// Account identifier in Privatbank system
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Account friendly name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Currency name according to ISO 4217
        /// </summary>
        public string Currency { get; }

        /// <summary>
        /// Account balance
        /// </summary>
        public decimal Balance { get; }

        /// <summary>
        /// Account credit limit
        /// </summary>
        public decimal CreditLimit { get; }
    }
}
