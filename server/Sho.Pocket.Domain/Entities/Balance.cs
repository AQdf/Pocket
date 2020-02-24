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
            Guid exchangeRateId,
            Guid userId)
        {
            Id = id;
            AssetId = assetId;
            EffectiveDate = effectiveDate;
            Value = value;
            ExchangeRateId = exchangeRateId;
            UserOpenId = userId;
        }

        public Guid Id { get; set; }

        public Guid AssetId { get; set; }

        public DateTime EffectiveDate { get; set; }

        public decimal Value { get; set; }

        public Guid ExchangeRateId { get; set; }

        public Guid UserOpenId { get; set; }

        public virtual Asset Asset { get; set; }

        public virtual ExchangeRate ExchangeRate { get; set; }
    }
}