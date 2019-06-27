using System;

namespace Sho.Pocket.Application.Balances.Models
{
    public class BalanceExportModel
    {
        public BalanceExportModel(DateTime effectiveDate, string asset, decimal value, string currency, decimal exchangeRate)
        {
            EffectiveDate = effectiveDate;
            Asset = asset;
            Value = value;
            Currency = currency;
            ExchangeRate = exchangeRate;
        }

        public DateTime EffectiveDate { get; set; }

        public string Asset { get; set; }

        public decimal Value { get; set; }

        public string Currency { get; set; }

        public decimal ExchangeRate { get; set; }

        public decimal DefaultCurrencyValue
        {
            get
            {
                return Value * ExchangeRate;
            }
        }
    }
}
