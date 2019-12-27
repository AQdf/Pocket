using System;

namespace Sho.Pocket.Domain.Entities
{
    public class AssetBankAccount : BaseEntity
    {
        public Guid Id { get; set; }

        public Guid AssetId { get; set; }

        public string BankName { get; set; }

        public string BankAccountId { get; set; }

        public DateTime LastSyncDateTime { get; set; }

        public string BankAccountName { get; set; }

        public string Token { get; set; }

        public string BankClientId { get; set; }
    }
}
