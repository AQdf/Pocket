using Sho.Pocket.Core.Features.BankSync.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.Features.BankSync.Abstractions
{
    public interface IBankAccountService
    {
        string BankName { get; }

        Task<IReadOnlyCollection<BankAccountBalance>> GetAccountsAsync(BankAccountsRequestParams requestParams);

        Task<IReadOnlyCollection<AccountTransaction>> GetAccountTransactionsAsync(AccountStatementRequestParams requestParams);
    }
}
