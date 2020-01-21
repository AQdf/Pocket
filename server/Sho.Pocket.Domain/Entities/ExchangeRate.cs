using System;

namespace Sho.Pocket.Domain.Entities
{
    public class ExchangeRate : BaseEntity
    {
        public Guid Id { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string BaseCurrency { get; set; }

        public string CounterCurrency { get; set; }

        public decimal BuyRate { get; set; }

        public decimal SellRate { get; set; }

        public string Provider { get; set; }
    }
}