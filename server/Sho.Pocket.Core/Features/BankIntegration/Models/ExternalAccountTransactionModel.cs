using System;

namespace Sho.Pocket.Core.Features.BankIntegration.Models
{
    public class ExternalAccountTransactionModel
    {
        public ExternalAccountTransactionModel(string id, DateTime date, string description, string currency, decimal amount, decimal balance)
        {
            Id = id;
            TransactionDate = date;
            Description = description;
            Currency = currency;
            Amount = amount;
            Balance = balance;
        }

        /// <summary>
        /// Unique identifier of the transaction in external system.
        /// </summary>
        public string Id { get; set; }

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
