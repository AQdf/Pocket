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
            AssetTypeId = asset.Type.Id;
            AssetTypeName = asset.Type.Name;
            CurrencyId = asset.Currency.Id;
            CurrencyName = asset.Currency.Name;
            Balance = asset.Balance;
            ExchangeRateId = asset.ExchangeRateId;
            IsActive = asset.IsActive;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid AssetTypeId { get; set; }

        public string AssetTypeName { get; set; }

        public Guid CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public decimal Balance { get; set; }

        public Guid? ExchangeRateId { get; set; }

        public bool IsActive { get; set; }
    }
}