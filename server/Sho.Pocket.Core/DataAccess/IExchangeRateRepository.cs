﻿using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IExchangeRateRepository
    {
        Task<ExchangeRate> AlterAsync(DateTime effectiveDate, string baseCurrency, string counterCurrency, decimal buy, decimal sell, string provider = null);

        Task<ExchangeRate> UpdateAsync(Guid id, decimal buy, decimal sell);

        Task<ExchangeRate> GetCurrencyExchangeRateAsync(string baseCurrency, DateTime effectiveDate);

        Task<IEnumerable<ExchangeRate>> GetByEffectiveDateAsync(DateTime effectiveDate);
    }
}