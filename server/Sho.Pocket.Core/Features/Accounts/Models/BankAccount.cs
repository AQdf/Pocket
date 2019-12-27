using System;

namespace Sho.Pocket.Core.Features.Accounts.Models
{
    public class BankAccount
    {
        public BankAccount(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
