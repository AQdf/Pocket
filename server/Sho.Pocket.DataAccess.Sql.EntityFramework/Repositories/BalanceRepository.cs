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
    public class BalanceRepository : IBalanceRepository
    {
        private readonly DbSet<Balance> _set;

        public BalanceRepository(PocketDbContext context)
        {
            _set = context.Set<Balance>();
        }

        public async Task<IEnumerable<Balance>> GetAllAsync(Guid userOpenId, bool includeRelated = true)
        {
            IQueryable<Balance> query = _set.Where(b => b.UserOpenId == userOpenId);

            if (includeRelated)
            {
                AddIncludeRelatedQuery(query);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Balance>> GetLatestBalancesAsync(Guid userOpenId, bool includeRelated = true)
        {
            Balance effectiveBalance = await _set
                .Where(b => b.UserOpenId == userOpenId)
                .OrderByDescending(b => b.EffectiveDate)
                .FirstOrDefaultAsync();

            if (effectiveBalance == null)
            {
                return new List<Balance>();
            }

            IQueryable<Balance> query = _set
                .Where(b => b.UserOpenId == userOpenId && b.EffectiveDate == effectiveBalance.EffectiveDate);

            if (includeRelated)
            {
                AddIncludeRelatedQuery(query);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Balance>> GetByEffectiveDateAsync(Guid userOpenId, DateTime effectiveDate, bool includeRelated = true)
        {
            IQueryable<Balance> query = _set.Where(b => b.UserOpenId == userOpenId && b.EffectiveDate == effectiveDate.Date);

            if (includeRelated)
            {
                AddIncludeRelatedQuery(query);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<DateTime>> GetOrderedEffectiveDatesAsync(Guid userOpenId)
        {
            return await _set
                .Where(b => b.UserOpenId == userOpenId)
                .Select(b => b.EffectiveDate)
                .OrderByDescending(date => date)
                .ToListAsync();
        }

        public async Task<Balance> GetByIdAsync(Guid userOpenId, Guid id)
        {
            return await _set.FirstOrDefaultAsync(b => b.Id == id && b.UserOpenId == userOpenId);
        }

        public async Task<Balance> CreateAsync(Guid userOpenId, Guid assetId, DateTime effectiveDate, decimal value, Guid exchangeRateId)
        {
            Balance balance = new Balance(Guid.NewGuid(), assetId, effectiveDate, value, exchangeRateId, userOpenId);
            EntityEntry<Balance> result = await _set.AddAsync(balance);

            return result.Entity;
        }

        public async Task<Balance> UpdateAsync(Guid userOpenId, Guid id, Guid assetId, decimal value)
        {
            Balance balance = await _set.SingleAsync(b => b.Id == id && b.UserOpenId == userOpenId);
            balance.AssetId = assetId;
            balance.Value = value;
            EntityEntry<Balance> result = _set.Update(balance);

            return result.Entity;
        }

        public async Task<bool> RemoveAsync(Guid userOpenId, Guid id)
        {
            Balance balance = await _set.SingleAsync(b => b.Id == id && b.UserOpenId == userOpenId);
            _set.Remove(balance);

            return true;
        }

        public async Task<bool> ExistsEffectiveDateBalancesAsync(Guid userOpenId, DateTime effectiveDate)
        {
            return await _set.AnyAsync(b => b.UserOpenId == userOpenId && b.EffectiveDate == effectiveDate.Date);
        }

        public async Task<bool> ExistsAssetBalanceAsync(Guid userOpenId, Guid assetId)
        {
            return await _set.AnyAsync(b => b.UserOpenId == userOpenId && b.AssetId == assetId);
        }

        private void AddIncludeRelatedQuery(IQueryable<Balance> query)
        {
            query.Include(b => b.Asset)
                .Include(b => b.ExchangeRate);
        }
    }
}
