using Newtonsoft.Json;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Application.Common.Converters;
using System;

namespace Sho.Pocket.Application.Balances.Models
{
    public class BalanceViewModel
    {
        public Guid? Id { get; set; }

        public Guid AssetId { get; set; }

        [JsonConverter(typeof(IsoStringDateTimeConverter))]
        public DateTime EffectiveDate { get; set; }

        public decimal Value { get; set; }

        public Guid ExchangeRateId { get; set; }

        public decimal ExchangeRateValue { get; set; }

        public decimal DefaultCurrencyValue { get; set; }

        public AssetViewModel Asset { get; set; }
    }
}
