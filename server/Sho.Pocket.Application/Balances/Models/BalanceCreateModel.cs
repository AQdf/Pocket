using Newtonsoft.Json;
using Sho.Pocket.Application.Common.Converters;
using System;

namespace Sho.Pocket.Application.Balances.Models
{
    public class BalanceCreateModel
    {
        public Guid AssetId { get; set; }

        [JsonConverter(typeof(IsoStringDateTimeConverter))]
        public DateTime EffectiveDate { get; set; }

        public decimal Value { get; set; }

        public Guid ExchangeRateId { get; set; }
    }
}
