using System.Collections.Generic;
using Sho.Pocket.Core.Features.ExchangeRates.Abstractions;

namespace Sho.Pocket.Application.ExchangeRates.Abstractions
{
    public interface IExchangeRateProviderResolver
    {
        IExchangeRateProvider Resolve(string providerName);

        IReadOnlyCollection<IExchangeRateProvider> GetActiveProviders();
    }
}
