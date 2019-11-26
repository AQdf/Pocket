using Sho.Pocket.Core.Configuration.Models;
using System.Collections.Generic;

namespace Sho.Pocket.Application.ExchangeRates.Abstractions
{
    public interface IExchangeRateProviderFactory
    {
        List<ExchangeRateProviderOption> GetActiveProvidersOptions();

        IExchangeRateProvider GetProvider(ExchangeRateProviderOption providerSettings);

        IExchangeRateProvider GetProvider(string providerName);
    }
}
