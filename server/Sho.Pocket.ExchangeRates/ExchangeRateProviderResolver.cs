using System;
using System.Collections.Generic;
using System.Linq;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Core.Features.ExchangeRates.Abstractions;
using Sho.Pocket.ExchangeRates.Configuration.Models;

namespace Sho.Pocket.ExchangeRates.Providers
{
    public class ExchangeRateProviderResolver : IExchangeRateProviderResolver
    {
        private readonly IEnumerable<IExchangeRateProvider> _exchangeRateProviders;

        private readonly ExchangeRateSettings _settings;

        public ExchangeRateProviderResolver(IEnumerable<IExchangeRateProvider> exchangeRateProviders, ExchangeRateSettings settings)
        {
            if (settings == null || settings.Providers == null)
            {
                throw new Exception("Missing exchange rate providers settings");
            }

            _exchangeRateProviders = exchangeRateProviders;
            _settings = settings;
        }

        public IExchangeRateProvider Resolve(string name)
        {
            IExchangeRateProvider provider = _exchangeRateProviders.FirstOrDefault(p => p.ProviderName.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (provider == null)
            {
                throw new Exception("Exchange Rate Provider is not supported");
            }

            return provider;
        }

        public IReadOnlyCollection<IExchangeRateProvider> GetActiveProviders()
        {
            List<IExchangeRateProvider> providers = _settings.Providers
                .Where(o => o.IsActive)
                .OrderBy(o => o.Priority)
                .Select(o => _exchangeRateProviders.FirstOrDefault(p => p.ProviderName.Equals(o.Name, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            return providers;
        }
    }
}
