using System;
using Sho.Pocket.Core.Features.ExchangeRates.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.Features.Balances.Models
{
    public class BalancePrimaryCurrencyModel
    {
        public BalancePrimaryCurrencyModel(string primaryCurrency, Balance balance, ExchangeRateModel exchangeRate)
        {
            AssetName = balance.Asset.Name;
            PrimaryCurrency = primaryCurrency;
            EffectiveDate = balance.EffectiveDate;
            Value = balance.Value;
            Rate = exchangeRate.Sell;
        }

        public string AssetName { get; set; }

        public string PrimaryCurrency { get; set; }

        public DateTime EffectiveDate { get; set; }

        public decimal Value { get; set; }

        public decimal Rate { get; set; }

        public decimal PrimaryCurrencyValue
        {
            get
            {
                return Value * Rate;
            }
        }
    }
}
