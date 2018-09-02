using System;

namespace Sho.Pocket.Domain.Entities
{
    public class ExchangeRate : BaseEntity
    {
        public DateTime EffectiveDate { get; set; }

        public Guid BaseCurrencyId { get; set; }

        public Guid CounterCurrencyId { get; set; }

        public decimal Rate { get; set; }

        public virtual Currency BaseCurrency { get; set; }

        public virtual Currency CounterCurrency { get; set; }
    }
}