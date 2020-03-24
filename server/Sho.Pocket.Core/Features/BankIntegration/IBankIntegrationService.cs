using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.BankIntegration.Models;

namespace Sho.Pocket.Core.Features.BankIntegration
{
    public interface IBankIntegrationService
    {
        string BankName { get; }

        Task<IReadOnlyCollection<ExternalAccountBalanceModel>> GetAccountsAsync(BankAccountsRequestParams requestParams);

        Task<IReadOnlyCollection<ExternalAccountTransactionModel>> GetAccountTransactionsAsync(AccountStatementRequestParams requestParams);
    }
}
