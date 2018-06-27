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
        
        public Guid PeriodId { get; set; }

        public Guid AssetTemplateId { get; set; }

        public override string ToString()
        {
            return $"{Name}: {Balance} {CurrencyName} | {TypeName}".Trim();
        }
    }

    public enum AssetType
    {
        Bank,
        Cash,
        ForeignExchange
    }
}
