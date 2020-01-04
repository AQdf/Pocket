using System;

namespace Sho.BankIntegration.Privatbank.Models
{
    public class PrivatbankAccountTransaction
    {
        public PrivatbankAccountTransaction(string card, string appCode, DateTime transactionDate, decimal amount, string currency, decimal balance, string description)
        {
            Card = card;
            AppCode = appCode;
            TransactionDate = transactionDate;
            Amount = amount;
            Currency = currency;
            Balance = balance;
            Description = description;
        }

        public string Card { get; }

        public string AppCode { get; }

        public DateTime TransactionDate { get; }

        public decimal Amount { get; }

        public string Currency { get; set; }

        public decimal Balance { get; }

        public string Description { get; }
    }
}
