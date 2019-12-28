using Sho.BankIntegration.Privatbank;
using Sho.BankIntegration.Privatbank.Models;
using Sho.BankIntegration.Privatbank.Services;
using Sho.Pocket.Core.Features.BankSync.Abstractions;
using Sho.Pocket.Core.Features.BankSync.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.BankIntegration
{
    public class PrivatbankAccountServiceAdapter : IBankAccountService
    {
        public string BankName => PrivatbankConfiguration.BANK_NAME;

        private readonly PrivatbankAccountService _accountService = new PrivatbankAccountService();

        public async Task<IReadOnlyCollection<BankAccountBalance>> GetClientAccountsInfoAsync(BankClientData client)
        {
            PrivatbankAccount account = await _accountService.GetMerchantAccountAsync(client.Token, client.Id, client.CardNumber);
            BankAccountBalance balance = new BankAccountBalance(BankName, account.Id, account.Name, account.Currency, account.Balance);

            return new List<BankAccountBalance> { balance };
        }

        public Task<string> GetClientAccountExctractAsync(string token)
        {
            throw new System.NotImplementedException();
        }
    }
}
