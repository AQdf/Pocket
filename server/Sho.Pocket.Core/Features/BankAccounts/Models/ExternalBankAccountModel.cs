namespace Sho.Pocket.Core.Features.BankAccounts.Models
{
    public class ExternalBankAccountModel
    {
        public ExternalBankAccountModel(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
