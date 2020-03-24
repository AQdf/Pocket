using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.BankIntegration.Privatbank;
using Sho.BankIntegration.Privatbank.Models;
using Sho.BankIntegration.Privatbank.Services;
using Sho.Pocket.Core.Features.BankIntegration;
using Sho.Pocket.Core.Features.BankIntegration.Models;

namespace Sho.Pocket.BankIntegration.Providers
{
    public class PrivatbankIntegrationService : IBankIntegrationService
    {
        public string BankName => PrivatbankConfig.BANK_NAME;

        private readonly PrivatbankAccountService _accountService;

        public PrivatbankIntegrationService(PrivatbankAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IReadOnlyCollection<ExternalAccountBalanceModel>> GetAccountsAsync(BankAccountsRequestParams request)
        {
            PrivatbankAccount account = await _accountService.GetMerchantAccountAsync(request.Token, request.BankClientId, request.CardNumber);
            ExternalAccountBalanceModel balance = new ExternalAccountBalanceModel(BankName, account.Id, account.Currency, account.Balance);

            return new List<ExternalAccountBalanceModel> { balance };
        }

        public async Task<IReadOnlyCollection<ExternalAccountTransactionModel>> GetAccountTransactionsAsync(AccountStatementRequestParams requestParams)
        {
            PrivatbankAccountStatement statement = await _accountService.GetMerchantAccountStatementAsync(
                requestParams.Token, requestParams.BankClientId, requestParams.Account, requestParams.From, requestParams.To);

            if (statement == null || statement.Items == null)
            {
                return new List<ExternalAccountTransactionModel>();
            }

            List<ExternalAccountTransactionModel> transactions = statement.Items
                .Select(i => new ExternalAccountTransactionModel(
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
