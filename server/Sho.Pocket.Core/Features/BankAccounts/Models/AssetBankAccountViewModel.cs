using Sho.Pocket.Domain.Entities;
using System;

namespace Sho.Pocket.Core.Features.BankAccounts.Models
{
    public class AssetBankAccountViewModel
    {
        public AssetBankAccountViewModel()
        {
        }

        public AssetBankAccountViewModel(AssetBankAccount account)
        {
            AssetId = account.AssetId;
            BankName = account.BankName;
            BankAccountName = account.BankAccountName;
            BankAccountIdMask = GetMask(account.BankAccountId);
            TokenMask = GetMask(account.Token);
            BankClientId = account.BankClientId;
        }

        public Guid AssetId { get; set; }

        public string BankName { get; set; }

        public string BankAccountName { get; set; }

        public string BankAccountIdMask { get; set; }

        public string TokenMask { get; set; }

        public string BankClientId { get; set; }

        private string GetMask(string input)
        {
            if (!string.IsNullOrWhiteSpace(input) && input.Length > 4)
            {
                string visiblePart = input.Substring(0, 4);
                string maskedPart = new string('*', input.Length - 4);

                return $"{visiblePart}{maskedPart}";
            }
            else
            {
                return input;
            }
        }
    }
}
