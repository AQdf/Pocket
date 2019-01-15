using Sho.Pocket.Domain.Entities;
using System;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IExchangeRateRepository
    {
        ExchangeRate Add(ExchangeRate exchangeRate);

        ExchangeRate Alter(DateTime effectiveDate, Guid baseCurrencyId, decimal rate);

        void Update(Guid id, decimal rate);
    }
}