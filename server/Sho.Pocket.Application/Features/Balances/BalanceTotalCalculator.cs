using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.Balances.Abstractions;
using Sho.Pocket.Core.Features.Balances.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Features.Balances
{
    public class BalanceTotalCalculator : IBalanceTotalCalculator
    {
        private readonly IExchangeRateRepository _exchangeRateRepository;

        public BalanceTotalCalculator(IExchangeRateRepository exchangeRateRepository)
        {
            _exchangeRateRepository = exchangeRateRepository;
        }

        public async Task<BalanceTotalModel> CalculateAsync(
            IEnumerable<Balance> balances,
            string currency,
            string primaryCurrency,
            DateTime effectiveDate)
        {
            decimal total = 0.0M;
            IEnumerable<ExchangeRate> rates = await _exchangeRateRepository.GetByEffectiveDateAsync(effectiveDate);
            var groups = balances.GroupBy(b => b.Asset.Currency);

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

        private decimal BuyPrimaryCurrency(IEnumerable<ExchangeRate> rates, string currency, string primaryCurrency, decimal value)
        {
            ExchangeRate rate = rates
                .FirstOrDefault(r => string.Equals(r.BaseCurrency, currency, StringComparison.OrdinalIgnoreCase)
                                  && string.Equals(r.CounterCurrency, primaryCurrency, StringComparison.OrdinalIgnoreCase));

            return value * rate.BuyRate;
        }

        private decimal SellPrimaryCurrency(IEnumerable<ExchangeRate> rates, string currency, string primaryCurrency, decimal value)
        {
            ExchangeRate rate = rates
                .FirstOrDefault(r => string.Equals(r.BaseCurrency, currency, StringComparison.OrdinalIgnoreCase)
                                  && string.Equals(r.CounterCurrency, primaryCurrency, StringComparison.OrdinalIgnoreCase));

            return value / rate.SellRate;
        }
    }
}
