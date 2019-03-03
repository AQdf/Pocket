using System;

namespace Sho.Pocket.Application.Balances.Models
{
    public class BalanceExportModel
    {
        public DateTime EffectiveDate { get; set; }

        public string Asset { get; set; }

        public decimal Value{ get; set; }

        public string Currency { get; set; }

        public decimal ExchangeRate { get; set; }

        public decimal DefaultCurrencyValue { get; set; }
    }
}
