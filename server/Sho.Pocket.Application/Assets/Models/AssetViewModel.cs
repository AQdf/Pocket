using Sho.Pocket.Domain.Entities;
using System;

namespace Sho.Pocket.Application.Assets.Models
{
    public class AssetViewModel
    {
        public AssetViewModel()
        {
        }

        public AssetViewModel(Guid id, string name, Guid currencyId, bool isActive)
        {
            Id = id;
            Name = name;
            CurrencyId = currencyId;
            IsActive = isActive;
        }

        public AssetViewModel(Asset asset)
        {
            Id = asset.Id;
            Name = asset.Name;
            CurrencyId = asset.CurrencyId;
            CurrencyName = asset.CurrencyName;
            IsActive = asset.IsActive;
        }

        public Guid? Id { get; set; }

        public string Name { get; set; }

        public Guid CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public bool IsActive { get; set; }
    }
}