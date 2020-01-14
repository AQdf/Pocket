using System;

namespace Sho.BankIntegration.Privatbank.Models
{
    /// <summary>
    /// Privatbank account transaction.
    /// </summary>
    public class PrivatbankAccountTransaction
    {
        public PrivatbankAccountTransaction(
            string id,
            string appCode,
            DateTime date,
            PrivatbankAmount cardAmount,
            PrivatbankAmount transactionAmount,
            PrivatbankAmount balance,
            string description,
            string terminal)
        {
            Id = id;
            AppCode = appCode;
            TransactionDate = date;
            TransactionAmount = transactionAmount;
            CardAmount = cardAmount;
            AccountBalance = balance;
            Description = description;
            Terminal = terminal;
        }

        /// <summary>
        /// Account identifier in Privatbank system.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Example: 591969.
        /// </summary>
        public string AppCode { get; }

        /// <summary>
        /// Date and time of the transaction.
        /// </summary>
        public DateTime TransactionDate { get; }

        /// <summary>
        /// Amount and currency of the transaction.
        /// </summary>
        public PrivatbankAmount TransactionAmount { get; }

        /// <summary>
        /// Amount and currency according to card's currency.
        /// </summary>
        public PrivatbankAmount CardAmount { get; }

        /// <summary>
        /// Account balance and currency after the transaction.
        /// </summary>
        public PrivatbankAmount AccountBalance { get; }

        /// <summary>
        /// Additional information about the transaction.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// System that made the transaction and its location.
        /// </summary>
        public string Terminal { get; }
    }
}
