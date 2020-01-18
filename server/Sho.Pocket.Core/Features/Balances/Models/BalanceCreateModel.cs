using Newtonsoft.Json;
using Sho.Pocket.Core.Common.Converters;
using System;

namespace Sho.Pocket.Core.Features.Balances.Models
{
    public class BalanceCreateModel
    {
        public BalanceCreateModel()
        {
        }

        public BalanceCreateModel(Guid assetId, DateTime effectiveDate, string currency, decimal value)
        {
            AssetId = assetId;
            EffectiveDate = effectiveDate;
            Currency = currency;
            Value = value;
        }

        public Guid AssetId { get; set; }

        [JsonConverter(typeof(IsoStringDateTimeConverter))]
        public DateTime EffectiveDate { get; set; }

        public string Currency { get; set; }

        public decimal Value { get; set; }
    }
}
