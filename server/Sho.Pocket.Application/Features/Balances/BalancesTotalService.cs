using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.Balances.Abstractions;
using Sho.Pocket.Core.Features.Balances.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Balances
{
    public class BalancesTotalService : IBalancesTotalService
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly IUserCurrencyRepository _userCurrencyRepository;

        public BalancesTotalService(
            IBalanceRepository balanceRepository,
            IExchangeRateRepository exchangeRateRepository,
            IUserCurrencyRepository userCurrencyRepository)
        {
            _balanceRepository = balanceRepository;
            _exchangeRateRepository = exchangeRateRepository;
            _userCurrencyRepository = userCurrencyRepository;
        }

        public async Task<List<BalanceTotalModel>> GetLatestTotalBalanceAsync(Guid userOpenId)
        {
            IEnumerable<DateTime> effectiveDates = await _balanceRepository.GetOrderedEffectiveDatesAsync(userOpenId);

            if (!effectiveDates.Any())
            {
                return null;
            }

            DateTime latestEffectiveDate = effectiveDates.FirstOrDefault();

            IEnumerable<Balance> balances = await _balanceRepository.GetByEffectiveDateAsync(userOpenId, latestEffectiveDate);

            if (!balances.Any())
            {
                return null;
            }

            IEnumerable<UserCurrency> userCurrencies = await _userCurrencyRepository.GetByUserIdAsync(userOpenId);
            string primaryCurrency = userCurrencies.Where(uc => uc.IsPrimary).Select(uc => uc.Currency).FirstOrDefault();

            IEnumerable<Task<BalanceTotalModel>> totalsCalculationTasks = userCurrencies
                .Select(uc => CalculateEffectiveDateTotalsAsync(balances, uc.Currency, primaryCurrency, latestEffectiveDate));

            BalanceTotalModel[] balanceTotals = await Task.WhenAll(totalsCalculationTasks);
            List<BalanceTotalModel> result = balanceTotals.OrderBy(uc => uc.Currency).ToList();

            return result;
        }

        public async Task<List<BalanceTotalChangeModel>> GetUserBalanceChangesAsync(Guid userOpenId, int count)
        {
            IEnumerable<DateTime> effectivateDates = await _balanceRepository.GetOrderedEffectiveDatesAsync(userOpenId);
            IEnumerable<DateTime> filterEffectiveDates = effectivateDates.Take(count);

            IEnumerable<UserCurrency> userCurrencies = await _userCurrencyRepository.GetByUserIdAsync(userOpenId);
            string primaryCurrency = userCurrencies.Where(uc => uc.IsPrimary).Select(uc => uc.Currency).FirstOrDefault();

            IEnumerable<Balance> balances = await _balanceRepository.GetAllAsync(userOpenId);
            List<Balance> balancesToDate = balances.Where(b => b.EffectiveDate <= filterEffectiveDates.Last()).ToList();

            List<BalanceTotalChangeModel> result = new List<BalanceTotalChangeModel>();

            foreach (var currency in userCurrencies)
            {
                List<BalanceTotalModel> values = new List<BalanceTotalModel>();

                foreach (var effectiveDate in filterEffectiveDates)
                {
                    var effectiveBalances = balances.Where(b => b.EffectiveDate == effectiveDate).ToList();
                    BalanceTotalModel balanceTotal = await CalculateEffectiveDateTotalsAsync(effectiveBalances, currency.Currency, primaryCurrency, effectiveDate);
                    values.Add(balanceTotal);
                }

                result.Add(new BalanceTotalChangeModel(currency.Currency, values));
            }

            return result;
        }

        public async Task<List<BalanceTotalModel>> CalculateTotalsAsync(Guid userOpenId, IEnumerable<Balance> balances, DateTime effectiveDate)
        {
            IEnumerable<UserCurrency> userCurrencies = await _userCurrencyRepository.GetByUserIdAsync(userOpenId);
            string primaryCurrency = userCurrencies.Where(uc => uc.IsPrimary).Select(uc => uc.Currency).FirstOrDefault();

            IEnumerable<Task<BalanceTotalModel>> totalsCalculationTasks = userCurrencies
                .Select(c => CalculateEffectiveDateTotalsAsync(balances, c.Currency, primaryCurrency, effectiveDate));

            BalanceTotalModel[] balanceTotals = await Task.WhenAll(totalsCalculationTasks);
            List<BalanceTotalModel> result = balanceTotals.OrderBy(c => c.Currency).ToList();

            return result;
        }

        public async Task<List<BalancePrimaryCurrencyModel>> GetUserPrimaryCurrencyBalancesAsync(Guid userOpenId)
        {
            UserCurrency userCurrency = await _userCurrencyRepository.GetPrimaryCurrencyAsync(userOpenId);
            IEnumerable<Balance> balances = await _balanceRepository.GetLatestBalancesAsync(userOpenId);

            List<BalancePrimaryCurrencyModel> result = balances
                .Select(b => new BalancePrimaryCurrencyModel(userCurrency.Currency, b))
                .ToList();

            return result;
        }

        private async Task<BalanceTotalModel> CalculateEffectiveDateTotalsAsync(IEnumerable<Balance> balances, string currency, string defaultCurrency, DateTime effectiveDate)
        {
            decimal value = 0;

            if (string.Equals(currency, defaultCurrency, StringComparison.InvariantCultureIgnoreCase))
            {
                value = balances.Select(b => b.Value * b.ExchangeRate?.Buy ?? 0).Sum();
            }
            else
            {
                List<Balance> currentCurrencyBalances = balances.Where(b => string.Equals(b.Asset.Currency, currency, StringComparison.InvariantCultureIgnoreCase)).ToList();
                List<Balance> defaultCurrencyBalances = balances.Where(b => string.Equals(b.Asset.Currency, defaultCurrency, StringComparison.InvariantCultureIgnoreCase)).ToList();
                List<Balance> otherCurrenciesBalances = balances.Where(b => !string.Equals(b.Asset.Currency, currency) && !string.Equals(b.Asset.Currency, defaultCurrency)).ToList();

                ExchangeRate currentCurrencyExchangeRate = await _exchangeRateRepository.GetCurrencyExchangeRate(currency, effectiveDate);

                value = currentCurrencyBalances.Select(b => b.Value).Sum()
                    + defaultCurrencyBalances.Select(b => b.Value / currentCurrencyExchangeRate.Buy).Sum()
                    + otherCurrenciesBalances.Select(b => b.Value * b.ExchangeRate.Buy / currentCurrencyExchangeRate.Buy).Sum();
            }

            BalanceTotalModel result = new BalanceTotalModel(effectiveDate, currency, value);

            return result;
        }
    }
}
