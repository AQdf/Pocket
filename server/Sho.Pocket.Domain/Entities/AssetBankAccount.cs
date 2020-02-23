using System;

namespace Sho.Pocket.Domain.Entities
{
    public class AssetBankAccount : BaseEntity
    {
        public AssetBankAccount()
        {
        }

        public AssetBankAccount(Guid id, Guid assetId, string bankName, string token, string bankClientId)
        {
            Id = id;
            AssetId = assetId;
            BankName = bankName;
            Token = token;
            BankClientId = bankClientId;
        }

        public Guid Id { get; set; }

        public Guid AssetId { get; set; }

        public string BankName { get; set; }

        public string BankAccountId { get; set; }

        public DateTime? LastSyncDateTime { get; set; }

        public string BankAccountName { get; set; }

        public string Token { get; set; }

        public string BankClientId { get; set; }

        public virtual Asset Asset { get; set; }
    }
}
