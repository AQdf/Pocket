using System.Collections.Generic;

namespace Sho.Pocket.Core.Features.ExchangeRates
{
    public interface IExchangeRateProviderResolver
    {
        IExchangeRateProvider Resolve(string providerName);

        IReadOnlyCollection<IExchangeRateProvider> GetActiveProviders();
    }
}
