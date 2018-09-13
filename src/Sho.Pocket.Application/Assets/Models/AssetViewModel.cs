using System;
using Sho.Pocket.Application.AssetTypes.Models;
using Sho.Pocket.Application.Currencies.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Assets.Models
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
            TypeId = asset.Type.Id;
            CurrencyId = asset.Currency.Id;
            IsActive = asset.IsActive;

            Type = new AssetTypeViewModel(asset.Type.Id, asset.Type.Name);
            Currency = new CurrencyViewModel(asset.Currency.Id, asset.Currency.Name);
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid TypeId { get; set; }

        public Guid CurrencyId { get; set; }

        public bool IsActive { get; set; }

        public AssetTypeViewModel Type { get; set; }

        public CurrencyViewModel Currency { get; set; }
    }
}