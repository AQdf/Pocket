using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Domain.Entities;
using Sho.Pocket.Domain.ValueObjects;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IAssetRepository
    {
        Task<IEnumerable<Asset>> GetByUserIdAsync(Guid userId, bool includeInactive);

        Task<Asset> GetByIdAsync(Guid userId, Guid id);

        Task<Asset> GetByNameAsync(Guid userId, string name);

        Task<Asset> CreateAsync(Guid userId, string name, Money balance, bool isActive, DateTime updatedOn);

        Task<Asset> UpdateAsync(Guid userId, Guid id, string name, Money balance, bool isActive, DateTime updatedOn);

        Task RemoveAsync(Guid userId, Guid assetId);
    }
}
