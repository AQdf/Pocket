using Sho.Pocket.Core.BankIntegration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.BankIntegration
{
    public interface IBankAccountService
    {
        Task<List<BankAccountBalance>> GetClientAccountsInfoAsync(string token);

        Task<string> GetClientAccountExctractAsync(string token);
    }
}
