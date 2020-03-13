using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.BankIntegration.Monobank;
using Sho.BankIntegration.Monobank.Models;
using Sho.BankIntegration.Monobank.Services;
using Sho.Pocket.Core.Features.ExchangeRates;
using Sho.Pocket.Core.Features.ExchangeRates.Models;

namespace Sho.Pocket.ExchangeRates.Providers
{
    public class MonobankExchangeRateProvider : IExchangeRateProvider
    {
        public string ProviderName => MonobankConfig.BANK_NAME;

        private readonly MonobankExchangeRateService _exchangeRateService;

        public MonobankExchangeRateProvider(MonobankExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        public async Task<List<ExchangeRateProviderModel>> FetchCurrencyRatesAsync(List<string> baseCurrencies, string counterCurrency)
        {
            IReadOnlyCollection<MonobankExchangeRate> rates = await _exchangeRateService.GetBankExchangeRatesAsync();

            List<ExchangeRateProviderModel> providerRates = rates
                .Where(r => counterCurrency.Equals(r.CounterCurrency.Name, StringComparison.OrdinalIgnoreCase)
                            && baseCurrencies.Contains(r.BaseCurrency.Name, StringComparer.OrdinalIgnoreCase))
                .Select(r => new ExchangeRateProviderModel(
                    r.Provider,
                    r.BaseCurrency.Name,
                    r.CounterCurrency.Name,
                    r.RateBuy ?? r.RateCross.Value,
                    r.RateSell ?? r.RateCross.Value))
                .ToList();

            return providerRates;
        }

        public async Task<ExchangeRateProviderModel> FetchRateAsync(string baseCurrency, string counterCurrency)
        {
            IReadOnlyCollection<MonobankExchangeRate> rates = await _exchangeRateService.GetBankExchangeRatesAsync();

            MonobankExchangeRate rate = rates.FirstOrDefault(r =>
                baseCurrency.Equals(r.BaseCurrency.Name, StringComparison.OrdinalIgnoreCase)
                && counterCurrency.Equals(r.CounterCurrency.Name, StringComparison.OrdinalIgnoreCase));

            if (rate != null)
            {
                return new ExchangeRateProviderModel(
                    rate.Provider,
                    rate.BaseCurrency.Name,
                    rate.CounterCurrency.Name,
                    rate.RateBuy ?? rate.RateCross.Value,
                    rate.RateSell ?? rate.RateCross.Value);
            }
            else
            {
                throw new Exception($"Failed to get {ProviderName} exchange rate from {baseCurrency} to {counterCurrency}.");
            }
        }
    }
}
