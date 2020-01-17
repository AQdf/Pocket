using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.BankIntegration.Privatbank;
using Sho.BankIntegration.Privatbank.Models;
using Sho.BankIntegration.Privatbank.Services;
using Sho.Pocket.Core.Features.BankAccounts.Abstractions;
using Sho.Pocket.Core.Features.BankAccounts.Models;

namespace Sho.Pocket.BankIntegration
{
    public class PrivatbankAccountServiceAdapter : IBankAccountService
    {
        public string BankName => PrivatbankConfig.BANK_NAME;

        private readonly PrivatbankAccountService _accountService;

        public PrivatbankAccountServiceAdapter(PrivatbankAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IReadOnlyCollection<BankAccountBalance>> GetAccountsAsync(BankAccountsRequestParams request)
        {
            PrivatbankAccount account = await _accountService.GetMerchantAccountAsync(request.Token, request.BankClientId, request.CardNumber);
            BankAccountBalance balance = new BankAccountBalance(BankName, account.Id, account.Currency, account.Balance);

            return new List<BankAccountBalance> { balance };
        }

        public async Task<IReadOnlyCollection<AccountTransaction>> GetAccountTransactionsAsync(AccountStatementRequestParams requestParams)
        {
            PrivatbankAccountStatement statement = await _accountService.GetMerchantAccountStatementAsync(
                requestParams.Token, requestParams.BankClientId, requestParams.Account, requestParams.From, requestParams.To);

            if (statement == null || statement.Items == null)
            {
                return new List<AccountTransaction>();
            }

            List<AccountTransaction> transactions = statement.Items
                .Select(i => new AccountTransaction(
                    i.AppCode,
                    i.TransactionDate,
                    i.Description,
                    i.TransactionAmount.Currency,
                    i.TransactionAmount.Value,
                    i.AccountBalance.Value))
                .ToList();

            return transactions;
        }
    }
}
