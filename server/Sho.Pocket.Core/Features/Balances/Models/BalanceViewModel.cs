using System;
using Newtonsoft.Json;
using Sho.Pocket.Core.Common.Converters;
using Sho.Pocket.Core.Features.Assets.Models;
using Sho.Pocket.Core.Features.ExchangeRates.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.Features.Balances.Models
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

            if (balance.ExchangeRate != null)
            {
                ExchangeRateValue = balance.ExchangeRate.Rate;
            }

            if (balance.Asset != null)
            {
                Asset = new AssetViewModel(balance.Asset);
            }
        }

        public BalanceViewModel(Balance balance, ExchangeRateModel exchangeRate, AssetViewModel asset)
        {
            Id = balance.Id;
            AssetId = balance.AssetId;
            EffectiveDate = balance.EffectiveDate;
            Value = balance.Value;
            ExchangeRateId = balance.ExchangeRateId;
            ExchangeRateValue = exchangeRate.Value;
            Asset = asset;
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

        public bool IsBankAccount { get; set; }
    }
}
