using System;

namespace Sho.Pocket.Api.Models
{
    public class AssetBankAccountUpdateRequest
    {
        public Guid AssetId { get; set; }

        public string BankName { get; set; }

        public string BankAccountId { get; set; }

        public string AccountName { get; set; }
    }
}
