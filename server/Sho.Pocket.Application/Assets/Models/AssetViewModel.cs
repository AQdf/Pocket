using System;
using Sho.Pocket.Application.AssetTypes.Models;
using Sho.Pocket.Application.Currencies.Models;

namespace Sho.Pocket.Application.Assets.Models
{
    public class AssetViewModel
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public Guid TypeId { get; set; }

        public Guid CurrencyId { get; set; }

        public bool IsActive { get; set; }

        public AssetTypeViewModel Type { get; set; }

        public CurrencyViewModel Currency { get; set; }
    }
}