using System;

namespace Sho.Pocket.Domain.Entities
{
    public class ExchangeRate : BaseEntity
    {
        public ExchangeRate()
        {
        }

        public ExchangeRate(Guid id, DateTime effectiveDate, string baseCurrency, string counterCurrency, decimal buyRate, decimal sellRate, string provider)
        {
            Id = id;
            EffectiveDate = effectiveDate;
            BaseCurrency = baseCurrency;
            CounterCurrency = counterCurrency;
            BuyRate = buyRate;
            SellRate = sellRate;
            Provider = provider;
        }

        public Guid Id { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string BaseCurrency { get; set; }

        public string CounterCurrency { get; set; }

        public decimal BuyRate { get; set; }

        public decimal SellRate { get; set; }

        public string Provider { get; set; }
    }
}