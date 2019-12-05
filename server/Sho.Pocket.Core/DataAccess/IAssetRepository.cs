using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IAssetRepository
    {
        Task<Asset> GetByIdAsync(Guid userOpenId, Guid id);

        Task<Asset> GetByNameAsync(Guid userOpenId, string name);

        Task<IEnumerable<Asset>> GetByUserIdAsync(Guid userOpenId);

        Task<Asset> CreateAsync(Guid userOpenId, string name, string currency, bool isActive);

        Task<Asset> UpdateAsync(Guid userOpenId, Guid id, string name, string currency, bool isActive);

        Task RemoveAsync(Guid userOpenId, Guid assetId);

        Task<IEnumerable<Asset>> GetActiveAssetsAsync();

        Task<bool> ExistsAssetBalanceAsync(Guid id);
    }
}
