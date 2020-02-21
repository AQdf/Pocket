using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.EntityFramework.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly DbSet<Asset> _set;

        public AssetRepository(PocketDbContext context)
        {
            _set = context.Set<Asset>();
        }

        public async Task<IEnumerable<Asset>> GetByUserIdAsync(Guid userOpenId, bool includeInactive)
        {
            IQueryable<Asset> query = _set.Where(a => a.UserOpenId == userOpenId);

            if (!includeInactive)
            {
                query.Where(a => a.IsActive);
            }

            return await query.ToListAsync();
        }

        public async Task<Asset> GetByIdAsync(Guid userOpenId, Guid id)
        {
            return await _set.SingleAsync(a => a.Id == id && a.UserOpenId == userOpenId);
        }

        public async Task<Asset> GetByNameAsync(Guid userOpenId, string name)
        {
            return await _set.SingleAsync(a => a.Name == name && a.UserOpenId == userOpenId);
        }

        public async Task<Asset> CreateAsync(Guid userOpenId, string name, string currency, bool isActive)
        {
            Asset asset = new Asset(Guid.NewGuid(), name, currency, isActive, userOpenId);
            EntityEntry<Asset> result = await _set.AddAsync(asset);

            return result.Entity;
        }

        public async Task<Asset> UpdateAsync(Guid userOpenId, Guid id, string name, string currency, bool isActive)
        {
            Asset asset = await _set.SingleAsync(a => a.Id == id && a.UserOpenId == userOpenId);
            asset.Name = name;
            asset.Currency = currency;
            asset.IsActive = isActive;
            _set.Update(asset);

            return asset;
        }

        public async Task RemoveAsync(Guid userOpenId, Guid id)
        {
            Asset asset = await _set.SingleAsync(a => a.Id == id && a.UserOpenId == userOpenId);
            _set.Remove(asset);
        }
    }
}
