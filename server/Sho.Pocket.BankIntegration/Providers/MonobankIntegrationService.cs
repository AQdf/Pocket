using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.BankIntegration.Monobank;
using Sho.BankIntegration.Monobank.Models;
using Sho.BankIntegration.Monobank.Services;
using Sho.Pocket.BankIntegration.Abstractions;
using Sho.Pocket.BankIntegration.Models;
using Sho.Pocket.Core.Features.BankAccounts.Models;

namespace Sho.Pocket.BankIntegration.Providers
{
    public class MonobankIntegrationService : IBankIntegrationService
    {
        public string BankName => MonobankConfig.BANK_NAME;

        private readonly MonobankAccountService _accountService;

        public MonobankIntegrationService(MonobankAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IReadOnlyCollection<ExternalAccountBalanceModel>> GetAccountsAsync(BankAccountsRequestParams requestParams)
        {
            MonobankClientInfo info = await _accountService.GetClientInfoAsync(requestParams.Token);

            List<ExternalAccountBalanceModel> balances = info.Accounts
                .Select(a => new ExternalAccountBalanceModel(BankName, a.Id, a.Currency.Name, a.Balance))
                .ToList();

            return balances;
        }

        public async Task<IReadOnlyCollection<ExternalAccountTransactionModel>> GetAccountTransactionsAsync(AccountStatementRequestParams requestParams)
        {
            IReadOnlyCollection<MonobankAccountTransaction> statementItems = 
                await _accountService.GetAccountTransactionsAsync(requestParams.Token, requestParams.Account, requestParams.From, requestParams.To);

            List<ExternalAccountTransactionModel> transactions = statementItems
                .Select(i => new ExternalAccountTransactionModel(i.Id, i.TransactionDate, i.Description, i.Currency.Name, i.Amount, i.Balance))
                .ToList();

            return transactions;
        }
    }
}
