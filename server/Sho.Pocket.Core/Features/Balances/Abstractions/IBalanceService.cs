using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.Balances.Models;

namespace Sho.Pocket.Core.Features.Balances.Abstractions
{
    public interface IBalanceService
    {
        Task<BalancesViewModel> GetUserLatestBalancesAsync(Guid userId);

        Task<BalancesViewModel> GetUserEffectiveBalancesAsync(Guid userId, DateTime effectiveDate);

        Task<BalanceViewModel> GetUserBalanceAsync(Guid userId, Guid id);

        Task<BalanceViewModel> AddBalanceAsync(Guid userId, BalanceCreateModel createModel);

        Task<List<BalanceViewModel>> AddEffectiveBalancesTemplate(Guid userId);

        Task<BalanceViewModel> UpdateBalanceAsync(Guid userId, Guid id, BalanceUpdateModel updateModel);

        Task<bool> DeleteBalanceAsync(Guid userId, Guid Id);

        Task<List<DateTime>> GetEffectiveDatesAsync(Guid userId);

        Task<BalanceViewModel> SyncBankAccountBalanceAsync(Guid userId, Guid id);
    }
}
