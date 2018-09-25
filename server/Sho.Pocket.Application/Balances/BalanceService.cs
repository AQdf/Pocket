using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Balances
{
    public class BalanceService : IBalanceService
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private IMapper _mapper;

        public BalanceService(
            IBalanceRepository balanceRepository,
            IAssetRepository assetRepository,
            IExchangeRateRepository exchangeRateRepository,
            IMapper mapper)
        {
            _balanceRepository = balanceRepository;
            _assetRepository = assetRepository;
            _exchangeRateRepository = exchangeRateRepository;
            _mapper = mapper;
        }

        public BalancesViewModel GetAll(DateTime? effectiveDate)
        {
            List<Balance> balances = _balanceRepository.GetAll();
            List<Asset> assets = _assetRepository.GetAll();

            if (effectiveDate.HasValue)
            {
                balances = balances.Where(b => b.EffectiveDate.Equals(effectiveDate.Value)).ToList();
            }

            balances.ForEach(b => b.Asset = assets.FirstOrDefault(a => b.AssetId == a.Id));

            List<BalanceViewModel> items = balances
                .Select(b => _mapper.Map<BalanceViewModel>(b))
                .ToList();

            decimal totalBalance = balances.Select(b => b.Value * b.ExchangeRate.Rate).Sum();

            return new BalancesViewModel(items, items.Count, totalBalance);
        }

        public BalanceViewModel GetById(Guid id)
        {
            Balance balance = _balanceRepository.GetById(id);

            BalanceViewModel result = _mapper.Map<BalanceViewModel>(balance);

            return result;
        }

        public void Add(BalanceViewModel balanceModel)
        {
            ExchangeRate exchangeRate = _exchangeRateRepository.Alter(balanceModel.EffectiveDate, balanceModel.AssetId, balanceModel.ExchangeRateValue);

            balanceModel.ExchangeRateId = exchangeRate.Id;

            Balance balance = _mapper.Map<Balance>(balanceModel);

            _balanceRepository.Add(balance);
        }

        public void Update(BalanceViewModel balanceModel)
        {
            ExchangeRate exchangeRate = _exchangeRateRepository.Alter(balanceModel.EffectiveDate, balanceModel.AssetId, balanceModel.ExchangeRateValue);

            balanceModel.ExchangeRateId = exchangeRate.Id;

            Balance balance = _mapper.Map<Balance>(balanceModel);

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
