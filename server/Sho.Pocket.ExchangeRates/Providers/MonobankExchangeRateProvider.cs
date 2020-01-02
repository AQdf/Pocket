using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.BankIntegration.Monobank;
using Sho.BankIntegration.Monobank.Models;
using Sho.BankIntegration.Monobank.Services;
using Sho.Pocket.Core.Features.ExchangeRates.Abstractions;
using Sho.Pocket.Core.Features.ExchangeRates.Models;

namespace Sho.Pocket.ExchangeRates.Providers
{
    public class MonobankExchangeRateProvider : IExchangeRateProvider
    {
        public string ProviderName => MonobankConfiguration.BANK_NAME;

        private readonly MonobankExchangeRateService _exchangeRateService = new MonobankExchangeRateService();

        public async Task<List<ExchangeRateProviderModel>> FetchCurrencyRatesAsync(List<string> baseCurrencies, string counterCurrency)
        {
            IReadOnlyCollection<MonobankExchangeRate> rates = await _exchangeRateService.GetBankExchangeRatesAsync();

            List<ExchangeRateProviderModel> providerRates = rates
                .Where(r => counterCurrency.Equals(r.CounterCurrency, StringComparison.OrdinalIgnoreCase) && baseCurrencies.Contains(r.BaseCurrency))
                .Select(r => new ExchangeRateProviderModel(r.Provider, r.BaseCurrency, r.CounterCurrency, r.RateSell ?? r.RateCross.Value))
                .ToList();

            return providerRates;
        }

        public async Task<ExchangeRateProviderModel> FetchRateAsync(string baseCurrency, string counterCurrency)
        {
            IReadOnlyCollection<MonobankExchangeRate> rates = await _exchangeRateService.GetBankExchangeRatesAsync();

            MonobankExchangeRate rate = rates.FirstOrDefault(r =>
                baseCurrency.Equals(r.BaseCurrency, StringComparison.OrdinalIgnoreCase)
                && counterCurrency.Equals(r.CounterCurrency, StringComparison.OrdinalIgnoreCase));

            if (rate != null)
            {
                return new ExchangeRateProviderModel(rate.Provider, rate.BaseCurrency, rate.CounterCurrency, rate.RateSell ?? rate.RateCross.Value);
            }
            else
            {
                // TODO: Log Monobank exchange rate not found!
                return null;
            }
        }
    }
}
