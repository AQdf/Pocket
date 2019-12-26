using Sho.Pocket.Core.BankIntegration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.BankIntegration
{
    public interface IBankAccountService
    {
        Task<List<BankAccountBalance>> GetClientAccountsInfoAsync(BankClientData clientData);

        Task<string> GetClientAccountExctractAsync(string token);
    }
}
