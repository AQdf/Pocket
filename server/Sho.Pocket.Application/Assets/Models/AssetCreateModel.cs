using System;

namespace Sho.Pocket.Application.Assets.Models
{
    public class AssetCreateModel
    {
        public string Name { get; set; }

        public Guid CurrencyId { get; set; }

        public bool IsActive { get; set; }
    }
}
