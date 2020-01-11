namespace Sho.BankIntegration.Monobank.Models
{
    /// <summary>
    /// Monobank account information.
    /// </summary>
    public class MonobankAccount
    {
        public MonobankAccount(string id, long balance, long creditLimit, int currencyCode, string cashbackType)
        {
            Id = id;
            Balance = (decimal)balance / 100;
            CreditLimit = (decimal)creditLimit / 100;
            Currency = new MonobankCurrency(currencyCode);
            CashbackType = cashbackType;
        }

        /// <summary>
        /// Account identifier in Monobank system.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Account balance.
        /// </summary>
        public decimal Balance { get; }

        /// <summary>
        /// Account credit limit.
        /// </summary>
        public decimal CreditLimit { get; }

        /// <summary>
        /// Currency according to ISO 4217.
        /// </summary>
        public MonobankCurrency Currency { get; }

        /// <summary>
        /// [None, UAH, Miles].
        /// </summary>
        public string CashbackType { get; }
    }
}
