using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IAssetRepository
    {
        Task<IEnumerable<Asset>> GetByUserIdAsync(Guid userOpenId, bool includeInactive);

        Task<Asset> GetByIdAsync(Guid userOpenId, Guid id);

        Task<Asset> GetByNameAsync(Guid userOpenId, string name);

        Task<Asset> CreateAsync(Guid userOpenId, string name, string currency, bool isActive);

        Task<Asset> UpdateAsync(Guid userOpenId, Guid id, string name, string currency, bool isActive);

        Task RemoveAsync(Guid userOpenId, Guid assetId);
    }
}
