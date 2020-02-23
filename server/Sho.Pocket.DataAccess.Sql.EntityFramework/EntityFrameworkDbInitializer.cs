using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.DataAccess.Configuration;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.EntityFramework
{
    public class EntityFrameworkDbInitializer : IDbInitializer
    {
        private readonly PocketDbContext _context;

        private readonly List<Currency> _defaultCurrencies;

        public EntityFrameworkDbInitializer(
            PocketDbContext context,
            IOptionsMonitor<DbSettings> options)
        {
            _context = context;

            _defaultCurrencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("UAH"),
                new Currency("EUR"),
                new Currency("PLN")
            };

            if (!string.IsNullOrWhiteSpace(options.CurrentValue.SystemDefaultCurrency)
                && !_defaultCurrencies.Exists(c => c.Name == options.CurrentValue.SystemDefaultCurrency))
            {
                _defaultCurrencies.Add(new Currency(options.CurrentValue.SystemDefaultCurrency));
            }
        }

        public void EnsureCreated()
        {
            try
            {
                _context.Database.EnsureCreated();
            }
            catch (System.Exception)
            {
                // Log error
            }
        }

        public void SeedStorageData()
        {
            AddDefaultCurrencies();
            _context.SaveChanges();
        }

        private void AddDefaultCurrencies()
        {
            DbSet<Currency> set = _context.Set<Currency>();
            List<Currency> dbCurrencies = set.ToList();
            IEnumerable<Currency> currenciesToInsert = _defaultCurrencies
                .Where(dc => !dbCurrencies.Any(c => c.Name.Equals(dc.Name, System.StringComparison.OrdinalIgnoreCase)));
            
            set.AddRange(currenciesToInsert);
        }
    }
}
