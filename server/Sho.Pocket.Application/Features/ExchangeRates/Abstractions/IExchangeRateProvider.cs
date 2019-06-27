using Sho.Pocket.Application.ExchangeRates.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Application.ExchangeRates.Abstractions
{
    public interface IExchangeRateProvider
    {
        string ProviderName { get; }

        Task<List<ExchangeRateProviderModel>> FetchCurrencyRatesAsync(List<string> baseCurrencies, string counterCurrency);

        Task<ExchangeRateProviderModel> FetchRateAsync(string baseCurrency, string counterCurrency);
    }
}
