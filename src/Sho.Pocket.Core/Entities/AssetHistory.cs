using System;

namespace Sho.Pocket.Core.Entities
{
    public class AssetHistory : BaseEntity
    {
        public AssetHistory()
        {
        }

        public AssetHistory(Asset asset)
        {
            AssetId = asset.Id;
            AssetName = asset.Name;
            EffectiveDate = DateTime.UtcNow;
            ExchangeRateId = null;
            Balance = asset.Balance;
        }

        public Guid AssetId { get; set; }

        public DateTime EffectiveDate { get; set; }

        public Guid? ExchangeRateId { get; set; }

        public decimal Balance { get; set; }

        public virtual string AssetName { get; set; }

        public virtual bool AssetIsActive { get; set; }
    }
}
