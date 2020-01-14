namespace Sho.BankIntegration.Privatbank.Models
{
    /// <summary>
    /// Privatbank account information.
    /// </summary>
    public class PrivatbankAccount
    {
        public PrivatbankAccount(string id, string name, string currency, decimal balance, decimal creditLimit)
        {
            Id = id;
            Name = name;
            Currency = currency;
            Balance = balance;
            CreditLimit = creditLimit;
        }

        /// <summary>
        /// Account identifier in Privatbank system.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Account name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Currency name according to ISO 4217.
        /// </summary>
        public string Currency { get; }

        /// <summary>
        /// Account balance.
        /// </summary>
        public decimal Balance { get; }

        /// <summary>
        /// Account credit limit.
        /// </summary>
        public decimal CreditLimit { get; }
    }
}
