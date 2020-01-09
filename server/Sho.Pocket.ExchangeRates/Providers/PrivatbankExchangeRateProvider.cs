using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.BankIntegration.Privatbank;
using Sho.BankIntegration.Privatbank.Models;
using Sho.BankIntegration.Privatbank.Services;
using Sho.Pocket.Core.Features.ExchangeRates.Abstractions;
using Sho.Pocket.Core.Features.ExchangeRates.Models;

namespace Sho.Pocket.ExchangeRates.Providers
{
    public class PrivatbankExchangeRateProvider : IExchangeRateProvider
    {
        public string ProviderName => PrivatbankDefaultConfig.BANK_NAME;

        private readonly PrivatbankExchangeRateService _exchangeRateService;

        public PrivatbankExchangeRateProvider(PrivatbankExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        public async Task<List<ExchangeRateProviderModel>> FetchCurrencyRatesAsync(List<string> baseCurrencies, string counterCurrency)
        {
            IReadOnlyCollection<PrivatbankExchangeRate> rates = await _exchangeRateService.GetBankExchangeRatesAsync();

            List<ExchangeRateProviderModel> providerRates = rates
                .Where(r => counterCurrency.Equals(r.CounterCurrency, StringComparison.OrdinalIgnoreCase) && baseCurrencies.Contains(r.BaseCurrency))
                .Select(r => new ExchangeRateProviderModel(r.Provider, r.BaseCurrency, r.CounterCurrency, r.Sell.Value))
                .ToList();

            return providerRates;
        }

        public async Task<ExchangeRateProviderModel> FetchRateAsync(string baseCurrency, string counterCurrency)
        {
            IReadOnlyCollection<PrivatbankExchangeRate> rates = await _exchangeRateService.GetBankExchangeRatesAsync();

            PrivatbankExchangeRate rate = rates.FirstOrDefault(r =>
                baseCurrency.Equals(r.BaseCurrency, StringComparison.OrdinalIgnoreCase)
                && counterCurrency.Equals(r.CounterCurrency, StringComparison.OrdinalIgnoreCase));

            return rate != null 
                ? new ExchangeRateProviderModel(rate.Provider, rate.BaseCurrency, rate.CounterCurrency, rate.Sell.Value)
                : null;
        }
    }
}
