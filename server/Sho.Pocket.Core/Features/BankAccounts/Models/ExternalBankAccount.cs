namespace Sho.Pocket.Core.Features.BankAccounts.Models
{
    public class ExternalBankAccount
    {
        public ExternalBankAccount(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
