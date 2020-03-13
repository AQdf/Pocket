using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.ExchangeRates.Models;

namespace Sho.Pocket.Core.Features.ExchangeRates
{
    public interface IExchangeRateProvider
    {
        string ProviderName { get; }

        Task<List<ExchangeRateProviderModel>> FetchCurrencyRatesAsync(List<string> baseCurrencies, string counterCurrency);

        Task<ExchangeRateProviderModel> FetchRateAsync(string baseCurrency, string counterCurrency);
    }
}
