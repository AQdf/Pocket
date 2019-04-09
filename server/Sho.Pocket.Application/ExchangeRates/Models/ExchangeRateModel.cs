using System;
using Newtonsoft.Json;
using Sho.Pocket.Application.Common.Converters;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.ExchangeRates.Models
{
    public class ExchangeRateModel
    {
        public ExchangeRateModel()
        {
        }

        public ExchangeRateModel(ExchangeRate exchangeRate)
        {
            Id = exchangeRate.Id;
            EffectiveDate = exchangeRate.EffectiveDate;
            BaseCurrencyId = exchangeRate.BaseCurrencyId;
            BaseCurrencyName = exchangeRate.BaseCurrencyName;
            CounterCurrencyId = exchangeRate.CounterCurrencyId;
            CounterCurrencyName = exchangeRate.CounterCurrencyName;
            Value = exchangeRate.Rate;
        }

        public Guid Id { get; set; }

        [JsonConverter(typeof(IsoStringDateTimeConverter))]
        public DateTime EffectiveDate { get; set; }

        public Guid BaseCurrencyId { get; set; }

        public string BaseCurrencyName { get; set; }

        public Guid CounterCurrencyId { get; set; }

        public string CounterCurrencyName { get; set; }

        public decimal Value { get; set; }
    }
}
