using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.Balances.Models;

namespace Sho.Pocket.Core.Features.Balances.Abstractions
{
    public interface IBalanceService
    {
        Task<BalancesViewModel> GetUserLatestBalancesAsync(Guid userOpenId);

        Task<BalancesViewModel> GetUserEffectiveBalancesAsync(Guid userOpenId, DateTime effectiveDate);

        Task<BalanceViewModel> GetUserBalanceAsync(Guid userOpenId, Guid id);

        Task<BalanceViewModel> AddBalanceAsync(Guid userOpenId, BalanceCreateModel createModel);

        Task<List<BalanceViewModel>> AddEffectiveBalancesTemplate(Guid userOpenId);

        Task<BalanceViewModel> UpdateBalanceAsync(Guid userOpenId, Guid id, BalanceUpdateModel updateModel);

        Task<bool> DeleteBalanceAsync(Guid userOpenId, Guid Id);

        Task<List<DateTime>> GetEffectiveDatesAsync(Guid userOpenId);

        Task<BalanceViewModel> SyncBankAccountBalanceAsync(Guid userOpenId, Guid id);
    }
}
