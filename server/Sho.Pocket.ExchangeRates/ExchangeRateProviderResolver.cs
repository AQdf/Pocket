using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Sho.Pocket.Core.Features.ExchangeRates;
using Sho.Pocket.ExchangeRates.Configuration.Models;

namespace Sho.Pocket.ExchangeRates
{
    public class ExchangeRateProviderResolver : IExchangeRateProviderResolver
    {
        private readonly IEnumerable<IExchangeRateProvider> _exchangeRateProviders;

        private readonly ExchangeRateSettings _settings;

        public ExchangeRateProviderResolver(
            IEnumerable<IExchangeRateProvider> exchangeRateProviders,
            IOptionsMonitor<ExchangeRateSettings> options)
        {
            if (options.CurrentValue == null || options.CurrentValue.Providers == null)
            {
                throw new Exception("Missing exchange rate providers settings");
            }

            _exchangeRateProviders = exchangeRateProviders;
            _settings = options.CurrentValue;
        }

        public IExchangeRateProvider Resolve(string name)
        {
            IExchangeRateProvider provider = _exchangeRateProviders
                .FirstOrDefault(p => p.ProviderName.Equals(name, StringComparison.OrdinalIgnoreCase));

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
