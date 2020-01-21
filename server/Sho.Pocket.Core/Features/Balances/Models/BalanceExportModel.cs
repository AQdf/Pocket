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
            ExchangeRateBuy = exchangeRate.Buy;
            ExchangeRateSell = exchangeRate.Sell;
            ExchangeRateProvider = exchangeRate.Provider;
            CounterCurrency = exchangeRate.CounterCurrency;
        }

        public DateTime EffectiveDate { get; set; }

        public string AssetName { get; set; }

        public decimal BalanceValue { get; set; }

        public string Currency { get; set; }

        public decimal ExchangeRateBuy { get; set; }

        public decimal ExchangeRateSell { get; set; }

        public string ExchangeRateProvider { get; set; }

        public string CounterCurrency { get; set; }
    }
}
