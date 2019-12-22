using System;

namespace Sho.Pocket.Core.Features.Accounts.Models
{
    public class BankAccount
    {
        public BankAccount(Guid authDataId, string id, string name)
        {
            AuthDataId = authDataId;
            Id = id;
            Name = name;
        }

        public Guid AuthDataId { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
