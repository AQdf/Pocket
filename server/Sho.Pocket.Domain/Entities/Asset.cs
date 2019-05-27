using System;

namespace Sho.Pocket.Domain.Entities
{
    public class Asset : BaseEntity
    {
        public Asset() {}

        public Asset(Guid id, string name, string currency, bool isActive)
        {
            Id = id;
            Name = name;
            Currency = currency;
            IsActive = isActive;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public bool IsActive { get; set; }
    }
}