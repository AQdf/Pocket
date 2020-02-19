using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.EntityFramework.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        public Task<Asset> CreateAsync(Guid userOpenId, string name, string currency, bool isActive)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAssetBalanceAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Asset>> GetActiveAssetsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Asset> GetByIdAsync(Guid userOpenId, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Asset> GetByNameAsync(Guid userOpenId, string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Asset>> GetByUserIdAsync(Guid userOpenId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Guid userOpenId, Guid assetId)
        {
            throw new NotImplementedException();
        }

        public Task<Asset> UpdateAsync(Guid userOpenId, Guid id, string name, string currency, bool isActive)
        {
            throw new NotImplementedException();
        }
    }
}
