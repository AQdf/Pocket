using System;

namespace Sho.Pocket.Application.Assets.Models
{
    public class AssetCreateModel
    {
        public AssetCreateModel()
        {
        }

        public AssetCreateModel(string name, Guid currencyId, bool isActive)
        {
            Name = name;
            CurrencyId = currencyId;
            IsActive = isActive;
        }

        public string Name { get; set; }

        public Guid CurrencyId { get; set; }

        public bool IsActive { get; set; }
    }
}
