namespace Sho.BankIntegration.Privatbank.Models
{
    internal class AccountStatementItemResponse
    {
        public AccountStatementItemResponse(string card, string appCode, string transactionDate, string transactionTime, string amount, string cardAmount, string rest, string terminal, string description)
        {
            Card = card;
            AppCode = appCode;
            TransactionDate = transactionDate;
            TransactionTime = transactionTime;
            Amount = amount;
            CardAmount = cardAmount;
            Rest = rest;
            Terminal = terminal;
            Description = description;
        }

        /// <summary>
        /// Card number.
        /// </summary>
        public string Card { get; }

        /// <summary>
        /// Example: 591969.
        /// </summary>
        public string AppCode { get; }

        /// <summary>
        /// Date of the transaction.
        /// Example 2013-09-02.
        /// </summary>
        public string TransactionDate { get; }

        /// <summary>
        /// Time of the transaction.
        /// </summary>
        public string TransactionTime { get; set; }

        /// <summary>
        /// Amount and currency of the transaction.
        /// </summary>
        public string Amount { get; }

        /// <summary>
        /// Amount and card currency of the transaction.
        /// </summary>
        public string CardAmount { get; }

        /// <summary>
        /// Card balance and currency after the transaction.
        /// </summary>
        public string Rest { get; }

        /// <summary>
        /// System that made the transaction and its location.
        /// </summary>
        public string Terminal { get; }

        /// <summary>
        /// Additional information about the transaction.
        /// </summary>
        public string Description { get; }
    }
}