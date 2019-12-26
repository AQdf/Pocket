using System;

namespace Sho.Pocket.Domain.Entities
{
    public class UserBankAuthData : BaseEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string BankName { get; set; }

        public string Token { get; set; }

        public string BankClientId { get; set; }
    }
}
