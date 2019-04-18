using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Core;

namespace Sho.Pocket.Application.ExchangeRates.Providers
{
    public class ExchangeRateProviderFactory : IExchangeRateProviderFactory
    {
        private readonly GlobalSettings _settings;

        public ExchangeRateProviderFactory(GlobalSettings settings)
        {
            _settings = settings;
        }

        public IExchangeRateProvider GetProvider(string name)
        {
            IExchangeRateProvider result;

            switch (name)
            {
                case ProviderConstants.DEFAULT_PROVIDER:
                    result = new DefaultExchangeRateProvider();
                    break;
                case ProviderConstants.FREE_CURRENCY_PROVIDER:
                    result = new FreeCurrencyConverterProvider(_settings);
                    break;
                default:
                    result = new DefaultExchangeRateProvider();
                    break;
            }

            return result;
        }
    }
}
