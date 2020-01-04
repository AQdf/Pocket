using Sho.BankIntegration.Privatbank;
using Sho.BankIntegration.Privatbank.Models;
using Sho.BankIntegration.Privatbank.Services;
using Sho.Pocket.Core.Features.BankSync.Abstractions;
using Sho.Pocket.Core.Features.BankSync.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sho.Pocket.BankIntegration
{
    public class PrivatbankAccountServiceAdapter : IBankAccountService
    {
        public string BankName => PrivatbankConfiguration.BANK_NAME;

        private readonly PrivatbankAccountService _accountService = new PrivatbankAccountService();

        public async Task<IReadOnlyCollection<BankAccountBalance>> GetAccountsAsync(BankAccountsRequestParams request)
        {
            PrivatbankAccount account = await _accountService.GetMerchantAccountAsync(request.Token, request.BankClientId, request.CardNumber);
            BankAccountBalance balance = new BankAccountBalance(BankName, account.Id, account.Name, account.Currency, account.Balance);

            return new List<BankAccountBalance> { balance };
        }

        public async Task<IReadOnlyCollection<AccountTransaction>> GetAccountTransactionsAsync(AccountStatementRequestParams requestParams)
        {
            IReadOnlyCollection<PrivatbankAccountTransaction> items = await _accountService.GetMerchantAccountTransactionsAsync(
                requestParams.Token, requestParams.BankClientId, requestParams.Account, requestParams.From, requestParams.To);

            List<AccountTransaction> transactions = items
                .Select(i => new AccountTransaction(i.AppCode, i.TransactionDate, i.Description, i.Currency, i.Amount, i.Balance))
                .ToList();

            return transactions;
        }
    }
}
