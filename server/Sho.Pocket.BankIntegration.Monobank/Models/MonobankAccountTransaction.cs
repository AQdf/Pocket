using System;

namespace Sho.BankIntegration.Monobank.Models
{
    /// <summary>
    /// Account transaction.
    /// </summary>
    public class MonobankAccountTransaction
    {
        public MonobankAccountTransaction(string id, DateTime date, string description, string currency, long amount, long balance)
        {
            Id = id;
            TransactionDate = date;
            Description = description;
            Currency = currency;
            Amount = (decimal)amount / 100;
            Balance = (decimal)balance / 100;
        }

        /// <summary>
        /// Unique identifier of the transaction in Monobank system.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Date and time of the transaction.
        /// </summary>
        public DateTime TransactionDate { get; }

        /// <summary>
        /// Transaction description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Currency name according to ISO 4217.
        /// </summary>
        public string Currency { get; }

        /// <summary>
        /// Amount in account currency.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Account balance in account currency after transaction.
        /// </summary>
        public decimal Balance { get; }
    }
}
