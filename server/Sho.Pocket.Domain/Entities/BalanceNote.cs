using System;

namespace Sho.Pocket.Domain.Entities
{
    public class BalanceNote : BaseEntity
    {
        public BalanceNote()
        {
        }

        public BalanceNote(Guid id, DateTime effectiveDate, string content, Guid userOpenId)
        {
            Id = id;
            EffectiveDate = effectiveDate;
            Content = content;
            UserOpenId = userOpenId;
        }

        public Guid Id { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string Content { get; set; }

        public Guid UserOpenId { get; set; }
    }
}
