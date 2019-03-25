using Newtonsoft.Json;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Application.Common.Converters;
using Sho.Pocket.Domain.Entities;
using System;

namespace Sho.Pocket.Application.Balances.Models
{
    public class BalanceViewModel
    {
        public BalanceViewModel()
        {
        }

        public BalanceViewModel(Balance balance)
        {
            Id = balance.Id;
            AssetId = balance.AssetId;
            EffectiveDate = balance.EffectiveDate;
            Value = balance.Value;
            ExchangeRateId = balance.ExchangeRateId;
            ExchangeRateValue = balance.ExchangeRate.Rate;
            Asset = new AssetViewModel(balance.Asset);
        }

        public Guid? Id { get; set; }

        public Guid AssetId { get; set; }

        [JsonConverter(typeof(IsoStringDateTimeConverter))]
        public DateTime EffectiveDate { get; set; }

        public decimal Value { get; set; }

        public Guid ExchangeRateId { get; set; }

        public decimal ExchangeRateValue { get; set; }

        public decimal DefaultCurrencyValue
        {
            get
            {
                return Value * ExchangeRateValue;
            }
        }

        public AssetViewModel Asset { get; set; }
    }
}
