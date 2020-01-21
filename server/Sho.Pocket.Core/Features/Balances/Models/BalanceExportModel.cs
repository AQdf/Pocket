using System;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.Features.Balances.Models
{
    public class BalanceExportModel
    {
        public BalanceExportModel(DateTime effectiveDate, Asset asset, ExchangeRate exchangeRate, decimal balanceValue)
        {
            EffectiveDate = effectiveDate;
            AssetName = asset.Name;
            BalanceValue = balanceValue;
            Currency = asset.Currency;
            BuyRate = exchangeRate.BuyRate;
            SellRate = exchangeRate.SellRate;
            RateProvider = exchangeRate.Provider;
            CounterCurrency = exchangeRate.CounterCurrency;
        }

        public DateTime EffectiveDate { get; set; }

        public string AssetName { get; set; }

        public decimal BalanceValue { get; set; }

        public string Currency { get; set; }

        public decimal BuyRate { get; set; }

        public decimal SellRate { get; set; }

        public string RateProvider { get; set; }

        public string CounterCurrency { get; set; }
    }
}
