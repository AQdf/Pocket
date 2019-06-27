using System;

namespace Sho.Pocket.Application.Balances.Models
{
    public class BalanceTotalModel
    {
        public BalanceTotalModel(DateTime effectiveDate, string currency, decimal value)
        {
            EffectiveDate = effectiveDate;
            Currency = currency;
            Value = value;
        }

        public DateTime EffectiveDate { get; set; }

        public string Currency { get; set; }

        public decimal Value { get; set; }
    }
}
