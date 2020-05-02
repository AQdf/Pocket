using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.Balances.Abstractions;
using Sho.Pocket.Core.Features.Balances.Models;
using Sho.Pocket.Core.Features.ExchangeRates;
using Sho.Pocket.Core.Features.ExchangeRates.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Features.Balances
{
    public class BalanceTotalCalculator : IBalanceTotalCalculator
    {
        private readonly IExchangeRateService _exchangeRateService;

        public BalanceTotalCalculator(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        public async Task<BalanceTotalModel> CalculateAsync(
            Guid userId,
            IEnumerable<Balance> balances,
            string currency,
            string primaryCurrency,
            DateTime effectiveDate)
        {
            decimal total = 0.0M;
            List<ExchangeRateModel> rates = await _exchangeRateService.GetExchangeRatesAsync(userId, effectiveDate);

            var groups = balances.GroupBy(b => b.Asset.Balance.Currency);

            foreach (IGrouping<string, Balance> g in groups)
            {
                decimal currencyTotal = g.Sum(b => b.Value);

                if (string.Equals(currency, g.Key, StringComparison.OrdinalIgnoreCase))
                {
                    total += currencyTotal;
                }
                else
                {
                    decimal primaryCurrencyValue = BuyPrimaryCurrency(rates, g.Key, primaryCurrency, currencyTotal);
                    decimal currencyValue = SellPrimaryCurrency(rates, currency, primaryCurrency, primaryCurrencyValue);

                    total += currencyValue;
                }
            }

            return new BalanceTotalModel(effectiveDate, currency, total);
        }

        private decimal BuyPrimaryCurrency(
            List<ExchangeRateModel> rates,
            string currency,
            string primaryCurrency,
            decimal value)
        {
            ExchangeRateModel rate = rates
                .FirstOrDefault(r => string.Equals(r.BaseCurrency, currency, StringComparison.OrdinalIgnoreCase)
                                  && string.Equals(r.CounterCurrency, primaryCurrency, StringComparison.OrdinalIgnoreCase));

            return value * rate.Buy;
        }

        private decimal SellPrimaryCurrency(
            List<ExchangeRateModel> rates,
            string currency,
            string primaryCurrency,
            decimal value)
        {
            ExchangeRateModel rate = rates
                .FirstOrDefault(r => string.Equals(r.BaseCurrency, currency, StringComparison.OrdinalIgnoreCase)
                                  && string.Equals(r.CounterCurrency, primaryCurrency, StringComparison.OrdinalIgnoreCase));

            return value / rate.Sell;
        }
    }
}
