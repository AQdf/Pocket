using System;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Assets.Models
{
    public class AssetViewModel
    {
        public AssetViewModel(Asset asset)
        {
            Id = asset.Id;
            Name = asset.Name;
            TypeId = asset.Type.Id;
            TypeName = asset.Type.Name;
            CurrencyId = asset.Currency.Id;
            CurrencyName = asset.Currency.Name;
            IsActive = asset.IsActive;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid TypeId { get; set; }

        public string TypeName { get; set; }

        public Guid CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public bool IsActive { get; set; }
    }
}