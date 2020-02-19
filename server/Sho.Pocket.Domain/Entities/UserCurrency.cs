using System;

namespace Sho.Pocket.Domain.Entities
{
    public class UserCurrency : BaseEntity
    {
        public UserCurrency()
        {
        }

        public UserCurrency(Guid userOpenId, string currency, bool isPrimary)
        {
            UserOpenId = userOpenId;
            Currency = currency;
            IsPrimary = isPrimary;
        }

        public Guid UserOpenId { get; set; }

        public string Currency { get; set; }

        public bool IsPrimary { get; set; }
    }
}
