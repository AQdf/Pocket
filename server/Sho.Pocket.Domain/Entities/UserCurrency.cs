using System;

namespace Sho.Pocket.Domain.Entities
{
    public class UserCurrency : BaseEntity
    {
        public Guid UserOpenId { get; set; }

        public string Currency { get; set; }

        public bool IsPrimary { get; set; }
    }
}
