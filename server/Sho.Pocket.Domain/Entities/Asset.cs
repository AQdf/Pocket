using Sho.Pocket.Domain.ValueObjects;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Domain.Entities
{
    public class Asset : BaseEntity
    {
        public Asset()
        {
        }

        public Asset(Guid id, string name, Money balance, bool isActive, Guid userId, DateTime updatedOn)
        {
            Id = id;
            Name = name;
            Balance = balance;
            IsActive = isActive;
            UserId = userId;
            UpdatedOn = updatedOn;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public Money Balance { get; set; }

        public bool IsActive { get; set; }

        public Guid UserId { get; set; }

        public DateTime UpdatedOn { get; set; }
        
        public virtual ICollection<Balance> Balances { get; set; }
    }
}