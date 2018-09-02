using System;

namespace Sho.Pocket.Domain.Entities
{
    public class Balance : BaseEntity
    {
        public Guid AssetId { get; set; }

        public DateTime EffectiveDate { get; set; }

        public decimal Value { get; set; }

        public Guid ExchangeRateId { get; set; }

        public virtual Asset Asset { get; set; }

        public virtual ExchangeRate ExchangeRate { get; set; }
    }
}