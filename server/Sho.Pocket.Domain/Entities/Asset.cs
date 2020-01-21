using System;

namespace Sho.Pocket.Domain.Entities
{
    public class Asset : BaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public bool IsActive { get; set; }
    }
}