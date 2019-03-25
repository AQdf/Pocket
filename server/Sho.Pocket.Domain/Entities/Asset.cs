using System;

namespace Sho.Pocket.Domain.Entities
{
    public class Asset : BaseEntity
    {
        public Asset() {}

        public Asset(Guid id, string name, Guid currencyId, bool isActive)
        {
            Id = id;
            Name = name;
            CurrencyId = currencyId;
            IsActive = isActive;
        }

        public string Name { get; set; }

        public Guid CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public bool IsActive { get; set; }
    }
}