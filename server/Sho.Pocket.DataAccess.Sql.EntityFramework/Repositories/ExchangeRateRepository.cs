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
    public class ExchangeRateRepository : IExchangeRateRepository
    {
        private readonly DbSet<ExchangeRate> _set;

        public ExchangeRateRepository(PocketDbContext context)
        {
            _set = context.Set<ExchangeRate>();
        }

        public async Task<IEnumerable<ExchangeRate>> GetByEffectiveDateAsync(DateTime effectiveDate)
        {
            return await _set
                .Where(r => r.EffectiveDate.Date == effectiveDate.Date)
                .ToListAsync();
        }

        public async Task<ExchangeRate> UpdateAsync(Guid id, decimal buy, decimal sell)
        {
            ExchangeRate exchangeRate = await _set.FirstOrDefaultAsync(r => r.Id == id);
            exchangeRate.BuyRate = buy;
            exchangeRate.SellRate = sell;
            EntityEntry<ExchangeRate> result = _set.Update(exchangeRate);

            return result.Entity;
        }

        public async Task<ExchangeRate> AlterAsync(DateTime effectiveDate, string baseCurrency, string counterCurrency, decimal buy, decimal sell, string provider = null)
        {
            ExchangeRate exchangeRate = await _set.FirstOrDefaultAsync(
                r => r.EffectiveDate.Date == effectiveDate.Date
                && r.BaseCurrency == baseCurrency
                && r.CounterCurrency == counterCurrency);

            EntityEntry<ExchangeRate> result;

            if (exchangeRate != null)
            {
                exchangeRate.BuyRate = buy;
                exchangeRate.SellRate = sell;
                exchangeRate.Provider = provider;
                result = _set.Update(exchangeRate);
            }
            else
            {
                ExchangeRate newRate = new ExchangeRate(Guid.NewGuid(), effectiveDate.Date, baseCurrency, counterCurrency, buy, sell, provider);
                result = await _set.AddAsync(newRate);
            }

            return result.Entity;
        }

        public async Task<ExchangeRate> GetCurrencyExchangeRateAsync(string baseCurrency, DateTime effectiveDate)
        {
            return await _set.FirstOrDefaultAsync(r => r.BaseCurrency == baseCurrency && r.EffectiveDate == effectiveDate.Date);
        }
    }
}
