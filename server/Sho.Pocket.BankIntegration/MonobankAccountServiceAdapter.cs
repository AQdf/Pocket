using Sho.BankIntegration.Monobank;
using Sho.BankIntegration.Monobank.Models;
using Sho.BankIntegration.Monobank.Services;
using Sho.Pocket.Core.Features.BankSync.Abstractions;
using Sho.Pocket.Core.Features.BankSync.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sho.Pocket.BankIntegration
{
    public class MonobankAccountServiceAdapter : IBankAccountService
    {
        public string BankName => MonobankConfiguration.BANK_NAME;

        private readonly MonobankAccountService _accountService = new MonobankAccountService();

        public async Task<IReadOnlyCollection<BankAccountBalance>> GetClientAccountsInfoAsync(BankClientData clientData)
        {
            IReadOnlyCollection<MonobankAccount> accounts = await _accountService.GetClientAccountsAsync(clientData.Token);

            List<BankAccountBalance> balances = accounts
                .Select(a => new BankAccountBalance(BankName, a.Id, a.Name, a.Currency, a.Balance))
                .ToList();

            return balances;
        }

        public Task<string> GetClientAccountExctractAsync(string token)
        {
            throw new System.NotImplementedException();
        }
    }
}
