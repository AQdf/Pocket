using System;

namespace Sho.Pocket.Core.Features.Accounts.Models
{
    public class AssetBankSyncData
    {
        public Guid AssetId { get; set; }

        public string BankName { get; set; }

        public string BankAccountName { get; set; }

        public string TokenMask { get; set; }
    }
}
