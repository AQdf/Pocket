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
            AssetName = balance.Asset.Name;
            AssetCurrencyId = balance.Asset.CurrencyId;
            AssetTypeId = balance.Asset.TypeId;
            EffectiveDate = balance.EffectiveDate;
            Value = balance.Value;
            ExchangeRateId = balance.ExchangeRateId;
            ExchangeRate = balance.ExchangeRate.Rate;
        }

        public Guid Id { get; set; }

        public Guid AssetId { get; set; }

        public string AssetName { get; set; }

        public Guid AssetCurrencyId { get; set; }

        public Guid AssetTypeId { get; set; }

        public DateTime EffectiveDate { get; set; }

        public decimal Value { get; set; }

        public Guid ExchangeRateId { get; set; }

        public decimal ExchangeRate { get; set; }
    }
}
