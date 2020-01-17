using System;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.Features.Assets.Models
{
    public class AssetViewModel
    {
        public AssetViewModel()
        {
        }

        public AssetViewModel(Guid id, string name, string currency, bool isActive)
        {
            Id = id;
            Name = name;
            Currency = currency;
            IsActive = isActive;
        }

        public AssetViewModel(Asset asset)
        {
            Id = asset.Id;
            Name = asset.Name;
            Currency = asset.Currency;
            IsActive = asset.IsActive;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public bool IsActive { get; set; }
    }
}