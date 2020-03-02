using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IBalanceRepository
    {
        Task<IEnumerable<Balance>> GetAllAsync(Guid userId);

        Task<IEnumerable<Balance>> GetByEffectiveDateAsync(Guid userId, DateTime effectiveDate);

        Task<IEnumerable<Balance>> GetLatestBalancesAsync(Guid userId);

        Task<IEnumerable<DateTime>> GetOrderedEffectiveDatesAsync(Guid userId);

        Task<Balance> GetByIdAsync(Guid userId, Guid id);

        Task<Balance> CreateAsync(Guid userId, Guid assetId, DateTime effectiveDate, decimal value);

        Task<Balance> UpdateAsync(Guid userId, Guid id, Guid assetId, decimal value);

        Task<bool> RemoveAsync(Guid userId, Guid balanceId);

        Task<bool> ExistsEffectiveDateBalancesAsync(Guid userId, DateTime effectiveDate);

        Task<bool> ExistsAssetBalanceAsync(Guid userId, Guid assetId);
    }
}
