using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IExchangeRateRepository
    {
        ExchangeRate Alter(DateTime effectiveDate, Guid baseCurrencyId, Guid counterCurrencyId, decimal rate);

        ExchangeRate Update(Guid id, decimal rate);

        ExchangeRate GetCurrencyExchangeRate(Guid baseCurrencyId, DateTime effectiveDate);

        List<ExchangeRate> GetByEffectiveDate(DateTime effectiveDate);
    }
}