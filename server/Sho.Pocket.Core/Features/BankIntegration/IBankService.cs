using System.Collections.Generic;
using Sho.Pocket.Core.Features.BankIntegration.Models;

namespace Sho.Pocket.Core.Features.BankIntegration
{
    public interface IBankService
    {
        IReadOnlyCollection<BankModel> GetBanks();

        BankModel GetBank(string bankName);
    }
}
