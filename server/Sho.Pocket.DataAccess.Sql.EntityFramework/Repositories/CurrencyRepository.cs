using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.EntityFramework.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly DbSet<Currency> _set;

        public CurrencyRepository(PocketDbContext context)
        {
            _set = context.Set<Currency>();
        }

        public async Task<Currency> CreateAsync(string name)
        {
            Currency currency = new Currency(name);
            EntityEntry<Currency> result = await _set.AddAsync(currency);

            return result.Entity;
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await _set.AnyAsync(c => c.Name == name);
        }

        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }
    }
}
