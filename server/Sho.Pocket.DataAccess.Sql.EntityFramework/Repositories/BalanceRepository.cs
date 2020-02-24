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

        public async Task<IEnumerable<Balance>> GetAllAsync(Guid userId, bool includeRelated = true)
        {
            List<Balance> result = await _set
                .Include(b => b.Asset)
                .Include(b => b.ExchangeRate)
                .Where(b => b.UserOpenId == userId)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Balance>> GetLatestBalancesAsync(Guid userId, bool includeRelated = true)
        {
            Balance effectiveBalance = await _set
                .Where(b => b.UserOpenId == userId)
                .OrderByDescending(b => b.EffectiveDate)
                .FirstOrDefaultAsync();

            if (effectiveBalance == null)
            {
                return new List<Balance>();
            }

            List<Balance> result = await _set
                .Include(b => b.Asset)
                .Include(b => b.ExchangeRate)
                .Where(b => b.UserOpenId == userId && b.EffectiveDate == effectiveBalance.EffectiveDate)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Balance>> GetByEffectiveDateAsync(Guid userId, DateTime effectiveDate, bool includeRelated = true)
        {
            List<Balance> result = await _set
                .Include(b => b.Asset)
                .Include(b => b.ExchangeRate)
                .Where(b => b.UserOpenId == userId && b.EffectiveDate == effectiveDate.Date)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<DateTime>> GetOrderedEffectiveDatesAsync(Guid userId)
        {
            List<DateTime> result = await _set
                .Where(b => b.UserOpenId == userId)
                .Select(b => b.EffectiveDate)
                .Distinct()
                .OrderByDescending(date => date)
                .ToListAsync();

            return result;
        }

        public async Task<Balance> GetByIdAsync(Guid userId, Guid id)
        {
            return await _set.FirstOrDefaultAsync(b => b.Id == id && b.UserOpenId == userId);
        }

        public async Task<Balance> CreateAsync(Guid userId, Guid assetId, DateTime effectiveDate, decimal value, Guid exchangeRateId)
        {
            Balance balance = new Balance(Guid.NewGuid(), assetId, effectiveDate, value, exchangeRateId, userId);
            EntityEntry<Balance> result = await _set.AddAsync(balance);

            return result.Entity;
        }

        public async Task<Balance> UpdateAsync(Guid userId, Guid id, Guid assetId, decimal value)
        {
            Balance balance = await _set.SingleAsync(b => b.Id == id && b.UserOpenId == userId);
            balance.AssetId = assetId;
            balance.Value = value;
            EntityEntry<Balance> result = _set.Update(balance);

            return result.Entity;
        }

        public async Task<bool> RemoveAsync(Guid userId, Guid id)
        {
            Balance balance = await _set.SingleAsync(b => b.Id == id && b.UserOpenId == userId);
            _set.Remove(balance);

            return true;
        }

        public async Task<bool> ExistsEffectiveDateBalancesAsync(Guid userId, DateTime effectiveDate)
        {
            return await _set.AnyAsync(b => b.UserOpenId == userId && b.EffectiveDate == effectiveDate.Date);
        }

        public async Task<bool> ExistsAssetBalanceAsync(Guid userId, Guid assetId)
        {
            return await _set.AnyAsync(b => b.UserOpenId == userId && b.AssetId == assetId);
        }
    }
}
