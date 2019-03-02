using System;

namespace Sho.Pocket.Domain.Entities
{
    public class Asset : BaseEntity
    {
        public string Name { get; set; }

        public Guid CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public bool IsActive { get; set; }
    }
}