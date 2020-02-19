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
    public class UserCurrencyRepository : IUserCurrencyRepository
    {
        private readonly DbSet<UserCurrency> _set;

        public UserCurrencyRepository(PocketDbContext context)
        {
            _set = context.Set<UserCurrency>();
        }

        public async Task<UserCurrency> CreateAsync(Guid userId, string currency, bool isPrimary)
        {
            UserCurrency userCurrency = new UserCurrency(userId, currency, isPrimary);
            EntityEntry<UserCurrency> result = await _set.AddAsync(userCurrency);

            return result.Entity;
        }

        public async Task<bool> DeleteAsync(Guid userId, string currency)
        {
            UserCurrency userCurrency = await _set.FirstOrDefaultAsync(uc => uc.Currency == currency && uc.UserOpenId == userId);

            if (userCurrency != null)
            {
                _set.Remove(userCurrency);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<UserCurrency>> GetByUserIdAsync(Guid userId)
        {
            return await _set.Where(uc => uc.UserOpenId == userId).ToListAsync();
        }

        public async Task<UserCurrency> GetAsync(Guid userId, string currency)
        {
            return await _set.FirstAsync(uc => uc.Currency == currency && uc.UserOpenId == userId);
        }

        public async Task<UserCurrency> GetPrimaryCurrencyAsync(Guid userId)
        {
            return await _set.FirstAsync(uc => uc.UserOpenId == userId && uc.IsPrimary);
        }

        public async Task<UserCurrency> SetPrimaryAsync(Guid userId, string currency)
        {
            List<UserCurrency> userCurrencies = await _set.Where(uc => uc.UserOpenId == userId).ToListAsync();
            userCurrencies.Select(uc => uc.IsPrimary = false);
            UserCurrency primary = userCurrencies.First(uc => uc.Currency == currency);

            return primary;
        }

        public async Task<bool> ExistsAsync(Guid userId, string currency)
        {
            return await _set.AnyAsync(uc => uc.Currency == currency && uc.UserOpenId == userId);
        }

        public Task<UserCurrency> GetCurrencyAsync(Guid userOpenId, string currency)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckIsPrimaryAsync(Guid userOpenId, string currency)
        {
            throw new NotImplementedException();
        }
    }
}
