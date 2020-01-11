namespace Sho.BankIntegration.Monobank.Models.Internal
{
    /// <summary>
    /// List of account transactions for the specified time.
    /// Referenece: <https://api.monobank.ua/docs/#/definitions/StatementItems>
    /// </summary>
    internal class StatementItem
    {
        /// <summary>
        /// Unique identifier of the transaction in Monobank system.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Time of the transaction in seconds in Unix time format.
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// Transaction description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Type of the transaction (Merchant Category Code) according to ISO 18245.
        /// Reference: <https://en.wikipedia.org/wiki/ISO_18245>.
        /// </summary>
        public int Mcc { get; set; }

        /// <summary>
        /// Transaction amount blocking status.
        /// </summary>
        public bool Hold { get; set; }

        /// <summary>
        /// Amount in account currency minimal units (cent, kopiyka).
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Amount in transaction currency minimal units (cent, kopiyka).
        /// </summary>
        public long OperationAmount { get; set; }

        /// <summary>
        /// Currency code according to ISO 4217.
        /// </summary>
        public int CurrencyCode { get; set; }

        /// <summary>
        /// Fee amount in currency minimal units (cent, kopiyka).
        /// </summary>
        public long CommissionRate { get; set; }

        /// <summary>
        /// Cashback amount in currency minimal units (cent, kopiyka).
        /// </summary>
        public long CashbackAmount { get; set; }

        /// <summary>
        /// Account balance in currency minimal units (cent, kopiyka).
        /// </summary>
        public long Balance { get; set; }
    }
}
