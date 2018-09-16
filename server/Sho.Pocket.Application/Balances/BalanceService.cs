using System;
using System.Collections.Generic;
using System.Linq;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Balances
{
    public class BalanceService : IBalanceService
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly IAssetRepository _assetRepository;

        public BalanceService(IBalanceRepository balanceRepository, IAssetRepository assetRepository)
        {
            _balanceRepository = balanceRepository;
            _assetRepository = assetRepository;
        }

        public BalancesViewModel GetAll(DateTime? effectiveDate)
        {
            IEnumerable<Balance> balances = _balanceRepository.GetAll();
            List<Asset> assets = _assetRepository.GetAll();

            if (effectiveDate.HasValue)
            {
                balances = balances.Where(b => b.EffectiveDate.Equals(effectiveDate.Value));
            }

            List<BalanceViewModel> items = balances
                .Select(b => new BalanceViewModel(b, assets.FirstOrDefault(a => b.AssetId == a.Id)))
                .ToList();

            decimal totalBalance = balances.Select(b => b.Value * b.ExchangeRate.Rate).Sum();

            return new BalancesViewModel(items, items.Count, totalBalance);
        }

        public void Add(BalanceViewModel balanceModel)
        {
            Balance balance = new Balance
            {
                AssetId = balanceModel.AssetId,
                EffectiveDate = balanceModel.EffectiveDate,
                ExchangeRateId = balanceModel.ExchangeRateId,
                Value = balanceModel.Value
            };

            _balanceRepository.Add(balance);
        }

        public void Update(BalanceViewModel balanceModel)
        {
            Balance balance = new Balance
            {
                Id = balanceModel.Id,
                AssetId = balanceModel.AssetId,
                EffectiveDate = balanceModel.EffectiveDate,
                ExchangeRateId = balanceModel.ExchangeRateId,
                Value = balanceModel.Value
            };

            _balanceRepository.Update(balance);
        }

        public void Delete(Guid Id)
        {
            _balanceRepository.Remove(Id);
        }

        public decimal GetTotalBalance()
        {
            List<Balance> balances = _balanceRepository.GetAll();

            DateTime latestEffectiveDate = balances
                .OrderByDescending(b => b.EffectiveDate)
                .Select(b => b.EffectiveDate)
                .FirstOrDefault();

            IEnumerable<Balance> effectiveBalances = balances.Where(b => b.EffectiveDate.Equals(latestEffectiveDate));

            decimal result = effectiveBalances.Select(b => b.Value * b.ExchangeRate.Rate).Sum();

            return result;
        }

        public IEnumerable<DateTime> GetEffectiveDates()
        {
            return _balanceRepository.GetEffectiveDates();
        }
    }
}
