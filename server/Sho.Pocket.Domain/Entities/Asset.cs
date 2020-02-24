using System;
using System.Collections.Generic;

namespace Sho.Pocket.Domain.Entities
{
    public class Asset : BaseEntity
    {
        public Asset()
        {
        }

        public Asset(Guid id, string name, string currency, bool isActive, Guid userId)
        {
            Id = id;
            Name = name;
            Currency = currency;
            IsActive = isActive;
            UserId = userId;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public bool IsActive { get; set; }

        public Guid UserId { get; set; }

        public virtual ICollection<Balance> Balances { get; set; }
    }
}