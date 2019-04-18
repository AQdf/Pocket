using Sho.Pocket.Application.ExchangeRates.Models;
using System.Collections.Generic;

namespace Sho.Pocket.Application.ExchangeRates.Abstractions
{
    public interface IExchangeRateProvider
    {
        string ProviderName { get; }

        List<ExchangeRateProviderModel> FetchCurrencyRates(List<string> baseCurrencies, string counterCurrency);

        ExchangeRateProviderModel FetchRate(string baseCurrency, string counterCurrency);
    }
}
