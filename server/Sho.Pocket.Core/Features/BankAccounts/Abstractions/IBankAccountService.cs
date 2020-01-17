using Sho.Pocket.Core.Features.BankAccounts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.Features.BankAccounts.Abstractions
{
    public interface IBankAccountService
    {
        string BankName { get; }

        Task<IReadOnlyCollection<BankAccountBalance>> GetAccountsAsync(BankAccountsRequestParams requestParams);

        Task<IReadOnlyCollection<AccountTransaction>> GetAccountTransactionsAsync(AccountStatementRequestParams requestParams);
    }
}
