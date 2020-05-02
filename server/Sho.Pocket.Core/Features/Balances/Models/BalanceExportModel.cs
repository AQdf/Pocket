using System;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.Features.Balances.Models
{
    public class BalanceExportModel
    {
        public BalanceExportModel(DateTime effectiveDate, Asset asset, decimal balanceValue)
        {
            EffectiveDate = effectiveDate;
            AssetName = asset.Name;
            BalanceValue = balanceValue;
            Currency = asset.Balance.Currency;
        }

        public DateTime EffectiveDate { get; set; }

        public string AssetName { get; set; }

        public decimal BalanceValue { get; set; }

        public string Currency { get; set; }
    }
}
