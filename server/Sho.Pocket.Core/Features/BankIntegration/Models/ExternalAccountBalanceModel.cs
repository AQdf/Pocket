﻿namespace Sho.Pocket.Core.Features.BankIntegration.Models
{
    public class ExternalAccountBalanceModel
    {
        public ExternalAccountBalanceModel(string bankName, string accountId, string currency, decimal balance)
        {
            BankName = bankName;
            AccountId = accountId;
            Currency = currency;
            Balance = balance;
        }

        public string BankName { get; set; }

        public string AccountId { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public string AccountName => $"{BankName}: {Balance} {Currency}";
    }
}
