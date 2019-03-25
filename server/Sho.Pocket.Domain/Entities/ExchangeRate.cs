using System;

namespace Sho.Pocket.Domain.Entities
{
    public class ExchangeRate : BaseEntity
    {
        public ExchangeRate() {}

        public ExchangeRate(DateTime effectiveDate, Guid baseCurrencyId, Guid counterCurrencyId, decimal rate)
        {
            EffectiveDate = effectiveDate;
            BaseCurrencyId = baseCurrencyId;
            CounterCurrencyId = counterCurrencyId;
            Rate = rate;
        }

        public DateTime EffectiveDate { get; set; }

        public Guid BaseCurrencyId { get; set; }

        public Guid CounterCurrencyId { get; set; }

        public decimal Rate { get; set; }

        public virtual Currency BaseCurrency { get; set; }

        public virtual Currency CounterCurrency { get; set; }
    }
}