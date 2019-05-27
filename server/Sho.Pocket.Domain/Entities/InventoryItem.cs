using System;

namespace Sho.Pocket.Domain.Entities
{
    public class InventoryItem : BaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public Guid UserOpenId { get; set; }
    }
}
