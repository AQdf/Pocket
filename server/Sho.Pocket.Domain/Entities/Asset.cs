using System;

namespace Sho.Pocket.Domain.Entities
{
    public class Asset : BaseEntity
    {
        public string Name { get; set; }

        public Guid TypeId { get; set; }

        public Guid CurrencyId { get; set; }

        public bool IsActive { get; set; }

        public virtual AssetType Type { get; set; }

        public virtual Currency Currency { get; set; }
    }
}