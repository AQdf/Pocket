using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.BankIntegration.Monobank;
using Sho.BankIntegration.Monobank.Models;
using Sho.BankIntegration.Monobank.Services;
using Sho.Pocket.Core.Features.BankSync.Abstractions;
using Sho.Pocket.Core.Features.BankSync.Models;

namespace Sho.Pocket.BankIntegration
{
    public class MonobankAccountServiceAdapter : IBankAccountService
    {
        public string BankName => MonobankDefaultConfig.BANK_NAME;

        private readonly MonobankAccountService _accountService;

        public MonobankAccountServiceAdapter(MonobankAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IReadOnlyCollection<BankAccountBalance>> GetAccountsAsync(BankAccountsRequestParams requestParams)
        {
            IReadOnlyCollection<MonobankAccount> accounts = await _accountService.GetAccountsAsync(requestParams.Token);

            List<BankAccountBalance> balances = accounts
                .Select(a => new BankAccountBalance(BankName, a.Id, a.Name, a.Currency, a.Balance))
                .ToList();

            return balances;
        }

        public async Task<IReadOnlyCollection<AccountTransaction>> GetAccountTransactionsAsync(AccountStatementRequestParams requestParams)
        {
            IReadOnlyCollection<MonobankAccountTransaction> statementItems = 
                await _accountService.GetAccountTransactionsAsync(requestParams.Token, requestParams.Account, requestParams.From, requestParams.To);

            List<AccountTransaction> transactions = statementItems
                .Select(i => new AccountTransaction(i.Id, i.TransactionDate, i.Description, i.Currency, i.Amount, i.Balance))
                .ToList();

            return transactions;
        }
    }
}
