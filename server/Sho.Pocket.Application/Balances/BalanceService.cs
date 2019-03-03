using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Application.Common.Comparers;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Constants;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Balances
{
    public class BalanceService : IBalanceService
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IBalanceExporter _balanceExporter;
        private readonly IMapper _mapper;

        public BalanceService(
            IBalanceRepository balanceRepository,
            IAssetRepository assetRepository,
            IExchangeRateRepository exchangeRateRepository,
            ICurrencyRepository currencyRepository,
            IBalanceExporter balanceExporter,
            IMapper mapper)
        {
            _balanceRepository = balanceRepository;
            _assetRepository = assetRepository;
            _exchangeRateRepository = exchangeRateRepository;
            _currencyRepository = currencyRepository;
            _balanceExporter = balanceExporter;
            _mapper = mapper;
        }

        public BalancesViewModel GetAll(DateTime effectiveDate)
        {
            List<Balance> balances = _balanceRepository.GetAll();
            List<Asset> assets = _assetRepository.GetAll();

            balances = balances.Where(b => b.EffectiveDate.Equals(effectiveDate)).ToList();

            balances.ForEach(b => b.Asset = assets.FirstOrDefault(a => b.AssetId == a.Id));

            List<BalanceViewModel> items = _mapper.Map<List<BalanceViewModel>>(balances);

            List<ExchangeRateModel> rates = balances
                .Select(b => _mapper.Map<ExchangeRateModel>(b.ExchangeRate))
                .Where(r => r.BaseCurrencyName != CurrencyConstants.UAH)
                .Distinct(new ExchangeRateComparer())
                .OrderBy(r => r.BaseCurrencyName)
                .ToList();

            IEnumerable<BalanceTotalModel> totals = CalculateTotals(balances, effectiveDate);

            items = items.OrderBy(i => i.Asset.Name).ToList();

            return new BalancesViewModel(items, items.Count, totals, rates);
        }

        public BalanceViewModel GetById(Guid id)
        {
            Balance balance = _balanceRepository.GetById(id);

            BalanceViewModel result = _mapper.Map<BalanceViewModel>(balance);

            return result;
        }

        public void Add(BalanceViewModel balanceModel)
        {
            ExchangeRate exchangeRate = _exchangeRateRepository.Alter(balanceModel.EffectiveDate, balanceModel.Asset.CurrencyId, balanceModel.ExchangeRateValue);

            balanceModel.ExchangeRateId = exchangeRate.Id;

            Balance balance = _mapper.Map<Balance>(balanceModel);

            _balanceRepository.Add(balance);
        }

        public bool AddEffectiveBalancesTemplate()
        {
            IEnumerable<DateTime> effectiveDates = GetEffectiveDates();
            DateTime today = DateTime.UtcNow.Date;
            bool todayBalancesExists = effectiveDates.Any(date => date.Equals(today));

            if (!todayBalancesExists)
            {
                _balanceRepository.AddEffectiveBalancesTemplate(today);
                return true;
            }

            return false;
        }

        public void Update(BalanceViewModel balanceModel)
        {
            Balance balance = _mapper.Map<Balance>(balanceModel);

            _balanceRepository.Update(balance);
        }

        public void Delete(Guid Id)
        {
            _balanceRepository.Remove(Id);
        }

        public IEnumerable<BalanceTotalModel> GetCurrentTotalBalance()
        {
            List<Balance> balances = _balanceRepository.GetAll();

            if (balances.Count == 0)
            {
                return null;
            }

            DateTime latestEffectiveDate = balances
                .OrderByDescending(b => b.EffectiveDate)
                .Select(b => b.EffectiveDate)
                .FirstOrDefault();

            IEnumerable<Balance> effectiveBalances = balances.Where(b => b.EffectiveDate.Equals(latestEffectiveDate));

            IEnumerable<BalanceTotalModel> result = CalculateTotals(effectiveBalances, latestEffectiveDate);

            result = result.Where(t => t.Currency == CurrencyConstants.UAH || t.Currency == CurrencyConstants.USD).ToList();

            return result;
        }

        public IEnumerable<DateTime> GetEffectiveDates()
        {
            return _balanceRepository.GetEffectiveDates();
        }

        public void ApplyExchangeRate(ExchangeRateModel model)
        {
            _exchangeRateRepository.Update(model.Id, model.Value);

            _balanceRepository.ApplyExchangeRate(model.Id, model.BaseCurrencyId, model.EffectiveDate);
        }

        public IEnumerable<BalanceTotalModel> GetCurrencyTotals(Guid currencyId, int count)
        {
            List<Balance> balances = _balanceRepository.GetAll();
            List<Asset> assets = _assetRepository.GetAll();
            balances.ForEach(b => b.Asset = assets.FirstOrDefault(a => b.AssetId == a.Id));

            List<DateTime> effectivateDates = _balanceRepository.GetEffectiveDates().Take(count).ToList();
            List<Currency> currencies = _currencyRepository.GetAll();
            Currency currency = currencies.FirstOrDefault(c => c.Id == currencyId);
            Guid defaultCurrencyId = currencies.Where(c => c.IsDefault).Select(c => c.Id).FirstOrDefault();

            var result = new List<BalanceTotalModel>();

            foreach (DateTime effectiveDate in effectivateDates)
            {
                var effectiveDateBalances = balances.Where(x => x.EffectiveDate == effectiveDate).ToList();

                BalanceTotalModel balanceTotal = CalculateCurrencyTotal(effectiveDateBalances, currency, defaultCurrencyId, effectiveDate);

                result.Add(balanceTotal);
            }

            result = result.OrderBy(x => x.EffectiveDate).ToList();

            return result;
        }

        public byte[] ExportBalancesToCsv()
        {
            List<Balance> balances = _balanceRepository.GetAll();
            List<Asset> assets = _assetRepository.GetAll();

            balances.ForEach(b => b.Asset = assets.FirstOrDefault(a => b.AssetId == a.Id));

            List<BalanceExportModel> items = _mapper.Map<List<BalanceExportModel>>(balances);

            string csv = _balanceExporter.ExportToCsv(items);
            byte[] bytes = Encoding.ASCII.GetBytes(csv);

            //using (var stream = new MemoryStream())
            //{
            //    var writeFile = new StreamWriter(stream);
            //    writeFile.Write(csv);
            //    bytes = stream.ToArray();
            //}

            return bytes;
        }

        private IEnumerable<BalanceTotalModel> CalculateTotals(IEnumerable<Balance> balances, DateTime effectiveDate)
        {
            List<BalanceTotalModel> result = new List<BalanceTotalModel>();

            List<Currency> currencies = _currencyRepository.GetAll();
            Guid defaultCurrencyId = currencies.Where(c => c.IsDefault).Select(c => c.Id).FirstOrDefault();
            
            foreach (Currency currency in currencies)
            {
                BalanceTotalModel balanceTotal = CalculateCurrencyTotal(balances, currency, defaultCurrencyId, effectiveDate);
                result.Add(balanceTotal);
            }

            result = result.OrderBy(t => t.Currency).ToList();

            return result;
        }

        private BalanceTotalModel CalculateCurrencyTotal(IEnumerable<Balance> balances, Currency currency, Guid defaultCurrencyId, DateTime effectiveDate)
        {
            decimal value = 0;

            if (currency.IsDefault)
            {
                value = balances.Select(b => b.Value * b.ExchangeRate?.Rate ?? 0).Sum();
            }
            else
            {
                List<Balance> currentCurrencyBalances = balances.Where(b => b.Asset.CurrencyId == currency.Id).ToList();
                List<Balance> defaultCurrencyBalances = balances.Where(b => b.Asset.CurrencyId == defaultCurrencyId).ToList();
                List<Balance> otherCurrenciesBalances = balances.Where(b => b.Asset.CurrencyId != currency.Id && b.Asset.CurrencyId != defaultCurrencyId).ToList();

                ExchangeRate currentCurrencyExchangeRate = _exchangeRateRepository.GetCurrencyExchangeRate(currency.Id, effectiveDate);

                value = currentCurrencyBalances.Select(b => b.Value).Sum()
                    + defaultCurrencyBalances.Select(b => b.Value / currentCurrencyExchangeRate.Rate).Sum()
                    + otherCurrenciesBalances.Select(b => b.Value * b.ExchangeRate.Rate / currentCurrencyExchangeRate.Rate).Sum();
            }

            BalanceTotalModel result = new BalanceTotalModel(effectiveDate, currency.Name, value);

            return result;
        }
    }
}
