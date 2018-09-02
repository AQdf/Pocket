using System;

namespace Sho.Pocket.Application.Assets.Models
{
    public class BalanceViewModel
    {
        public Guid Id { get; set; }

        public Guid AssetId { get; set; }

        public string AssetCurrency { get; set; }

        public string AssetType { get; set; }

        public DateTime EffectiveDate { get; set; }

        public decimal Value { get; set; }

        public Guid ExchangeRateId { get; set; }

        public decimal ExchangeRate { get; set; }
    }
}
