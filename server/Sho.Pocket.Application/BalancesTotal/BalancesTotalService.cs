using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Constants;
using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sho.Pocket.Application.BalancesTotal
{
    public class BalancesTotalService : IBalancesTotalService
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly ICurrencyRepository _currencyRepository;

        public BalancesTotalService(
            IBalanceRepository balanceRepository,
            IExchangeRateRepository exchangeRateRepository,
            ICurrencyRepository currencyRepository)
        {
            _balanceRepository = balanceRepository;
            _exchangeRateRepository = exchangeRateRepository;
            _currencyRepository = currencyRepository;
        }

        public async Task<List<BalanceTotalModel>> GetCurrentTotalBalance(Guid userOpenId)
        {
            IEnumerable<DateTime> effectiveDates = await _balanceRepository.GetOrderedEffectiveDatesAsync(userOpenId);
            DateTime latestEffectiveDate = effectiveDates.FirstOrDefault();
            IEnumerable<Balance> balances = await _balanceRepository.GetByEffectiveDateAsync(userOpenId, latestEffectiveDate);

            if (!balances.Any())
            {
                return null;
            }

            IEnumerable<BalanceTotalModel> totals = await CalculateTotalsAsync(balances, latestEffectiveDate);

            List<BalanceTotalModel> result = totals.OrderBy(t => t.Value)
                .Where(t => t.Currency == CurrencyConstants.UAH || t.Currency == CurrencyConstants.USD)
                .ToList();

            return result;
        }

        public async Task<List<BalanceTotalModel>> GetCurrencyTotals(Guid userOpenId, string currencyName, int count)
        {
            IEnumerable<DateTime> effectivateDates = await _balanceRepository.GetOrderedEffectiveDatesAsync(userOpenId);
            IEnumerable<DateTime> totalsEffectiveDates = effectivateDates.Take(count);

            bool exists = await _currencyRepository.ExistsAsync(currencyName);

            if (!exists)
            {
                throw new ArgumentException($"Specified currency {currencyName} is not supported in the system.");
            }

            List<BalanceTotalModel> result = new List<BalanceTotalModel>();

            foreach (DateTime effectiveDate in totalsEffectiveDates)
            {
                IEnumerable<Balance> balances = await _balanceRepository.GetByEffectiveDateAsync(userOpenId, effectiveDate);
                BalanceTotalModel balanceTotal = await CalculateCurrencyTotalAsync(balances, currencyName, CurrencyConstants.UAH, effectiveDate);
                result.Add(balanceTotal);
            }

            return result;
        }

        public async Task<List<BalanceTotalModel>> CalculateTotalsAsync(IEnumerable<Balance> balances, DateTime effectiveDate)
        {
            IEnumerable<Currency> currencies = await _currencyRepository.GetAllAsync();

            IEnumerable<Task<BalanceTotalModel>> totalsCalculationTasks = currencies
                .Select(c => CalculateCurrencyTotalAsync(balances, c.Name, CurrencyConstants.UAH, effectiveDate));

            BalanceTotalModel[] balanceTotals = await Task.WhenAll(totalsCalculationTasks);
            List<BalanceTotalModel> result = balanceTotals.OrderBy(c => c.Currency).ToList();

            return result;
        }

        private async Task<BalanceTotalModel> CalculateCurrencyTotalAsync(IEnumerable<Balance> balances, string currency, string defaultCurrency, DateTime effectiveDate)
        {
            decimal value = 0;

            if (string.Equals(currency, defaultCurrency, StringComparison.InvariantCultureIgnoreCase))
            {
                value = balances.Select(b => b.Value * b.ExchangeRate?.Rate ?? 0).Sum();
            }
            else
            {
                List<Balance> currentCurrencyBalances = balances.Where(b => string.Equals(b.Asset.Currency, currency, StringComparison.InvariantCultureIgnoreCase)).ToList();
                List<Balance> defaultCurrencyBalances = balances.Where(b => string.Equals(b.Asset.Currency, defaultCurrency, StringComparison.InvariantCultureIgnoreCase)).ToList();
                List<Balance> otherCurrenciesBalances = balances.Where(b => !string.Equals(b.Asset.Currency, currency) && !string.Equals(b.Asset.Currency, defaultCurrency)).ToList();

                ExchangeRate currentCurrencyExchangeRate = await _exchangeRateRepository.GetCurrencyExchangeRate(currency, effectiveDate);

                value = currentCurrencyBalances.Select(b => b.Value).Sum()
                    + defaultCurrencyBalances.Select(b => b.Value / currentCurrencyExchangeRate.Rate).Sum()
                    + otherCurrenciesBalances.Select(b => b.Value * b.ExchangeRate.Rate / currentCurrencyExchangeRate.Rate).Sum();
            }

            BalanceTotalModel result = new BalanceTotalModel(effectiveDate, currency, value);

            return result;
        }
    }
}
