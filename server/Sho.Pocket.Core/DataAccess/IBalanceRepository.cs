using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IBalanceRepository
    {
        Task<IEnumerable<Balance>> GetAllAsync(Guid userOpenId, bool includeRelated = true);

        Task<IEnumerable<Balance>> GetByEffectiveDateAsync(Guid userOpenId, DateTime effectiveDate, bool includeRelated = true);

        Task<IEnumerable<Balance>> GetLatestBalancesAsync(Guid userOpenId, bool includeRelated = true);

        Task<Balance> GetByIdAsync(Guid userOpenId, Guid id);

        Task<Balance> CreateAsync(Guid userOpenId, Guid assetId, DateTime effectiveDate, decimal value, Guid exchangeRateId);

        Task<IEnumerable<Balance>> AddEffectiveBalances(DateTime currentEffectiveDate);

        Task<Balance> UpdateAsync(Guid userOpenId, Guid id, Guid assetId, decimal value);

        Task<bool> RemoveAsync(Guid userOpenId, Guid balanceId);

        Task<IEnumerable<DateTime>> GetOrderedEffectiveDatesAsync(Guid userOpenId);

        Task<bool> ExistsEffectiveDateBalancesAsync(Guid userOpenId, DateTime effectiveDate);
    }
}
