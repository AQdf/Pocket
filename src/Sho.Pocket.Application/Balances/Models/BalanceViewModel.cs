using Newtonsoft.Json;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Application.Converters;
using Sho.Pocket.Domain.Entities;
using System;

namespace Sho.Pocket.Application.Balances.Models
{
    public class BalanceViewModel
    {
        public BalanceViewModel()
        {
        }

        public BalanceViewModel(Balance balance, Asset asset)
        {
            Id = balance.Id;
            AssetId = balance.AssetId;
            EffectiveDate = balance.EffectiveDate;
            Value = balance.Value;
            ExchangeRateId = balance.ExchangeRateId;
            ExchangeRate = balance.ExchangeRate.Rate;
            Asset = new AssetViewModel(asset);
        }

        public Guid Id { get; set; }

        public Guid AssetId { get; set; }

        [JsonConverter(typeof(IsoStringDateTimeConverter))]
        public DateTime EffectiveDate { get; set; }

        public decimal Value { get; set; }

        public Guid ExchangeRateId { get; set; }

        public decimal ExchangeRate { get; set; }

        public AssetViewModel Asset { get; set; }
    }
}
