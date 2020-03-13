using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.ExchangeRates.Models;

namespace Sho.Pocket.Core.Features.ExchangeRates
{
    public interface IExchangeRateExternalService
    {
        Task<IReadOnlyCollection<ExchangeRateProviderModel>> FetchProviderExchangeRateAsync(string providerName, string counterCurrency, List<string> baseCurrencies);

        Task<IReadOnlyCollection<ExchangeRateProviderModel>> TryFetchRatesAsync(string counterCurrecy, List<string> baseCurrencies);
    }
}
