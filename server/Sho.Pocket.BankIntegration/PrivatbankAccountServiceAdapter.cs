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

        public async Task<IReadOnlyCollection<BankAccountBalance>> GetAccountsAsync(BankAccountsRequestParams request)
        {
            PrivatbankAccount account = await _accountService.GetMerchantAccountAsync(request.Token, request.Id, request.CardNumber);
            BankAccountBalance balance = new BankAccountBalance(BankName, account.Id, account.Name, account.Currency, account.Balance);

            return new List<BankAccountBalance> { balance };
        }

        public Task<IReadOnlyCollection<AccountTransaction>> GetAccountTransactionsAsync(AccountStatementRequestParams requestParams)
        {
            throw new System.NotImplementedException();
        }
    }
}
