using Sho.Pocket.BankIntegration.Models;
using Sho.Pocket.Core.Features.BankAccounts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.BankIntegration.Abstractions
{
    public interface IBankIntegrationService
    {
        string BankName { get; }

        Task<IReadOnlyCollection<ExternalAccountBalanceModel>> GetAccountsAsync(BankAccountsRequestParams requestParams);

        Task<IReadOnlyCollection<ExternalAccountTransactionModel>> GetAccountTransactionsAsync(AccountStatementRequestParams requestParams);
    }
}
