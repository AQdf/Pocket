using System;

namespace Sho.Pocket.Core.Entities
{
    public class Asset : BaseEntity
    {
        public string Name { get; set; }
        
        public Guid TypeId { get; set; }

        public string TypeName { get; set; }
        
        public Guid CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public decimal Balance { get; set; }

        public Guid? ExchangeRateId { get; set; }
    }
}
