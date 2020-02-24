using Sho.Pocket.Core.Features.ExchangeRates.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Application.ExchangeRates.Abstractions
{
    public interface IExchangeRateService
    {
        Task<List<ExchangeRateModel>> GetExchangeRatesAsync(DateTime effectiveDate);

        Task<List<ExchangeRateModel>> AddDefaultExchangeRates(Guid userId, DateTime effectiveDate);

        Task UpdateExchangeRateAsync(ExchangeRateModel model);

        Task<IReadOnlyCollection<ExchangeRateProviderModel>> FetchProviderExchangeRateAsync(Guid userId, string providerName);
    }
}
