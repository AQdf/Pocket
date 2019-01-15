using System;
using Newtonsoft.Json;
using Sho.Pocket.Application.Common.Converters;

namespace Sho.Pocket.Application.ExchangeRates.Models
{
    public class ExchangeRateModel
    {
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
