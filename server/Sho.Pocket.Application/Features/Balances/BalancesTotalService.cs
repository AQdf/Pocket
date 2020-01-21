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

        private readonly IUserCurrencyRepository _userCurrencyRepository;

        private readonly IBalanceTotalCalculator _balanceTotalCalculator;

        public BalancesTotalService(
            IBalanceRepository balanceRepository,
            IUserCurrencyRepository userCurrencyRepository,
            IBalanceTotalCalculator balanceTotalCalculator)
        {
            _balanceRepository = balanceRepository;
            _userCurrencyRepository = userCurrencyRepository;
            _balanceTotalCalculator = balanceTotalCalculator;
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

            List<BalanceTotalModel> result = await CalculateTotalsAsync(userOpenId, balances, latestEffectiveDate);

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
                    BalanceTotalModel balanceTotal = await _balanceTotalCalculator.CalculateAsync(effectiveBalances, currency.Currency, primaryCurrency, effectiveDate);
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

            List<BalanceTotalModel> result = new List<BalanceTotalModel>();

            foreach (var uc in userCurrencies)
            {
                BalanceTotalModel totals = await _balanceTotalCalculator.CalculateAsync(balances, uc.Currency, primaryCurrency, effectiveDate);
                result.Add(totals);
            }

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
    }
}
