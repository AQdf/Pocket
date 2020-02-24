using System;

namespace Sho.Pocket.Domain.Entities
{
    public class UserCurrency : BaseEntity
    {
        public UserCurrency()
        {
        }

        public UserCurrency(Guid userId, string currency, bool isPrimary)
        {
            UserId = userId;
            Currency = currency;
            IsPrimary = isPrimary;
        }

        public Guid UserId { get; set; }

        public string Currency { get; set; }

        public bool IsPrimary { get; set; }
    }
}
