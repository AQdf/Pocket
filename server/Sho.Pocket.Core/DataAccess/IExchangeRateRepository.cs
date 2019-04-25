using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IExchangeRateRepository
    {
        Task<ExchangeRate> Alter(DateTime effectiveDate, Guid baseCurrencyId, Guid counterCurrencyId, decimal rate);

        Task<ExchangeRate> Update(Guid id, decimal rate);

        Task<ExchangeRate> GetCurrencyExchangeRate(Guid baseCurrencyId, DateTime effectiveDate);

        Task<IEnumerable<ExchangeRate>> GetByEffectiveDate(DateTime effectiveDate);

        Task<bool> Exists(Guid baseCurrencyId, DateTime effectiveDate);
    }
}