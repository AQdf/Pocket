using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.ExchangeRates.Models;

namespace Sho.Pocket.Core.Features.ExchangeRates
{
    public interface IExchangeRateService
    {
        Task<List<ExchangeRateModel>> GetExchangeRatesAsync(Guid userId, DateTime effectiveDate);

        Task<List<ExchangeRateModel>> AddExchangeRatesAsync(Guid userId, DateTime effectiveDate);

        Task UpdateExchangeRateAsync(ExchangeRateModel model);

        Task<IReadOnlyCollection<ExchangeRateProviderModel>> FetchProviderExchangeRatesAsync(Guid userId, string provider);
    }
}
