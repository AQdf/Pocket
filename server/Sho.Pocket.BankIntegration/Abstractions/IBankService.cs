using System.Collections.Generic;
using Sho.Pocket.BankIntegration.Models;

namespace Sho.Pocket.BankIntegration.Abstractions
{
    public interface IBankService
    {
        IReadOnlyCollection<BankModel> GetBanks();

        BankModel GetBank(string bankName);
    }
}
