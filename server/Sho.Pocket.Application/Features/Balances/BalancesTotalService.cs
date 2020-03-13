using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.Balances.Abstractions;
using Sho.Pocket.Core.Features.Balances.Models;
using Sho.Pocket.Core.Features.ExchangeRates;
using Sho.Pocket.Core.Features.ExchangeRates.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Balances
{
    public class BalancesTotalService : IBalancesTotalService
    {
        private readonly IExchangeRateService _exchangeRateService;

        private readonly IBalanceRepository _balanceRepository;

        private readonly IUserCurrencyRepository _userCurrencyRepository;

        private readonly IBalanceTotalCalculator _balanceTotalCalculator;

        public BalancesTotalService(
            IExchangeRateService exchangeRateService,
            IBalanceRepository balanceRepository,
            IUserCurrencyRepository userCurrencyRepository,
            IBalanceTotalCalculator balanceTotalCalculator)
        {
            _exchangeRateService = exchangeRateService;
            _balanceRepository = balanceRepository;
            _userCurrencyRepository = userCurrencyRepository;
            _balanceTotalCalculator = balanceTotalCalculator;
        }

        public async Task<List<BalanceTotalModel>> GetLatestTotalBalanceAsync(Guid userId)
        {
            IEnumerable<DateTime> effectiveDates = await _balanceRepository.GetOrderedEffectiveDatesAsync(userId);

            if (!effectiveDates.Any())
            {
                return null;
            }

            DateTime latestEffectiveDate = effectiveDates.FirstOrDefault();

            IEnumerable<Balance> balances = await _balanceRepository.GetByEffectiveDateAsync(userId, latestEffectiveDate);

            List<BalanceTotalModel> result = await CalculateTotalsAsync(userId, balances, latestEffectiveDate);

            return result;
        }

        public async Task<List<BalanceTotalChangeModel>> GetUserBalanceChangesAsync(Guid userId, int count)
        {
            IEnumerable<DateTime> effectivateDates = await _balanceRepository.GetOrderedEffectiveDatesAsync(userId);
            IEnumerable<DateTime> filterEffectiveDates = effectivateDates.Take(count);

            IEnumerable<UserCurrency> userCurrencies = await _userCurrencyRepository.GetByUserIdAsync(userId);
            string primaryCurrency = userCurrencies.Where(uc => uc.IsPrimary).Select(uc => uc.Currency).FirstOrDefault();

            IEnumerable<Balance> balances = await _balanceRepository.GetAllAsync(userId);
            List<Balance> balancesToDate = balances.Where(b => b.EffectiveDate <= filterEffectiveDates.Last()).ToList();

            List<BalanceTotalChangeModel> result = new List<BalanceTotalChangeModel>();

            foreach (var currency in userCurrencies)
            {
                List<BalanceTotalModel> values = new List<BalanceTotalModel>();

                foreach (var effectiveDate in filterEffectiveDates)
                {
                    var effectiveBalances = balances.Where(b => b.EffectiveDate == effectiveDate).ToList();

                    BalanceTotalModel balanceTotal = await _balanceTotalCalculator
                        .CalculateAsync(userId, effectiveBalances, currency.Currency, primaryCurrency, effectiveDate);

                    values.Add(balanceTotal);
                }

                result.Add(new BalanceTotalChangeModel(currency.Currency, values));
            }

            return result;
        }

        public async Task<List<BalanceTotalModel>> CalculateTotalsAsync(Guid userId, IEnumerable<Balance> balances, DateTime effectiveDate)
        {
            IEnumerable<UserCurrency> userCurrencies = await _userCurrencyRepository.GetByUserIdAsync(userId);
            string primaryCurrency = userCurrencies.Where(uc => uc.IsPrimary).Select(uc => uc.Currency).FirstOrDefault();

            List<BalanceTotalModel> result = new List<BalanceTotalModel>();

            foreach (var uc in userCurrencies)
            {
                BalanceTotalModel totals = await _balanceTotalCalculator
                    .CalculateAsync(userId, balances, uc.Currency, primaryCurrency, effectiveDate);
                result.Add(totals);
            }

            return result;
        }

        public async Task<List<BalancePrimaryCurrencyModel>> GetUserPrimaryCurrencyBalancesAsync(Guid userId)
        {
            UserCurrency userCurrency = await _userCurrencyRepository.GetPrimaryCurrencyAsync(userId);
            IEnumerable<Balance> balances = await _balanceRepository.GetLatestBalancesAsync(userId);
            List<BalancePrimaryCurrencyModel> result = new List<BalancePrimaryCurrencyModel>();

            if (balances.Count() == 0)
            {
                return result;
            }

            DateTime effectiveDate = balances.First().EffectiveDate;
            List<ExchangeRateModel> exchangeRates = await _exchangeRateService.GetExchangeRatesAsync(userId, effectiveDate);

            foreach (Balance balance in balances)
            {
                ExchangeRateModel rate = exchangeRates.First(r => 
                    r.BaseCurrency == balance.Asset.Currency
                    && r.CounterCurrency == userCurrency.Currency);

                result.Add(new BalancePrimaryCurrencyModel(userCurrency.Currency, balance, rate));
            }

            return result;
        }
    }
}
