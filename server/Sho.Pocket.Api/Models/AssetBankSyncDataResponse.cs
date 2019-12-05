using System;

namespace Sho.Pocket.Api.Models
{
    public class AssetBankSyncDataResponse
    {
        public Guid AssetId { get; set; }

        public string BankName { get; set; }

        public string BankAccountName { get; set; }

        public string TokenMask { get; set; }
    }
}
