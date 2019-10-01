using System;

namespace Sho.Pocket.Domain.Entities
{
    public class BalanceNote : BaseEntity
    {
        public Guid Id { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string Content { get; set; }
    }
}
