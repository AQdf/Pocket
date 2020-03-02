using System;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.Features.Assets.Models
{
    public class AssetViewModel
    {
        public AssetViewModel()
        {
        }

        public AssetViewModel(Asset asset)
        {
            Id = asset.Id;
            Name = asset.Name;
            Currency = asset.Currency;
            IsActive = asset.IsActive;
            Value = asset.Value;
            UpdatedOn = asset.UpdatedOn;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public bool IsActive { get; set; }

        public decimal Value { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}