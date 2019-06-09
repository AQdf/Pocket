using Sho.Pocket.Application.ExchangeRates.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Application.ExchangeRates.Abstractions
{
    public interface IExchangeRateService
    {
        Task<List<ExchangeRateModel>> AddDefaultExchangeRates(Guid userOpenId, DateTime effectiveDate);

        Task<ExchangeRateModel> AlterExchangeRateAsync(ExchangeRateModel model);

        Task UpdateExchangeRateAsync(ExchangeRateModel model);
    }
}
