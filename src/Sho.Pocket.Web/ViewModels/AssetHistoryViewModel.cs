using System;

namespace Sho.Pocket.Api.ViewModels
{
    public class AssetHistoryViewModel
    {
        public string AssetName { get; set; }

        public DateTime EffectiveDate { get; set; }

        public decimal Balance { get; set; }
    }
}
