using Newtonsoft.Json;
using Sho.Pocket.Core.Common.Converters;
using System;

namespace Sho.Pocket.Application.Balances.Models
{
    public class BalanceCreateModel
    {
        public BalanceCreateModel()
        {
        }

        public BalanceCreateModel(Guid assetId, DateTime effectiveDate, decimal value, Guid exchangeRateId)
        {
            AssetId = assetId;
            EffectiveDate = effectiveDate;
            Value = value;
            ExchangeRateId = exchangeRateId;
        }

        public Guid AssetId { get; set; }

        [JsonConverter(typeof(IsoStringDateTimeConverter))]
        public DateTime EffectiveDate { get; set; }

        public decimal Value { get; set; }

        public Guid ExchangeRateId { get; set; }
    }
}
