using System;

namespace Sho.Pocket.Domain.Entities
{
    public class Balance : BaseEntity
    {
        public Balance()
        {
        }

        public Balance(
            Guid id,
            Guid assetId,
            DateTime effectiveDate,
            decimal value,
            Guid userId)
        {
            Id = id;
            AssetId = assetId;
            EffectiveDate = effectiveDate;
            Value = value;
            UserId = userId;
        }

        public Guid Id { get; set; }

        public Guid AssetId { get; set; }

        public DateTime EffectiveDate { get; set; }

        public decimal Value { get; set; }

        public Guid UserId { get; set; }

        public virtual Asset Asset { get; set; }
    }
}