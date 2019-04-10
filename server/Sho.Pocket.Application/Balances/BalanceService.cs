using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Application.DataExport;
using Sho.Pocket.Application.ExchangeRates;
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
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ICsvExporter _csvExporter;

        public BalanceService(
            IBalanceRepository balanceRepository,
            IAssetRepository assetRepository,
            IExchangeRateRepository exchangeRateRepository,
            ICurrencyRepository currencyRepository,
            IExchangeRateService exchangeRateService,
            ICsvExporter balanceExporter)
        {
            _balanceRepository = balanceRepository;
            _assetRepository = assetRepository;
            _exchangeRateRepository = exchangeRateRepository;
            _currencyRepository = currencyRepository;
            _exchangeRateService = exchangeRateService;
            _csvExporter = balanceExporter;
        }

        public BalancesViewModel GetAll(DateTime effectiveDate)
        {
            List<Balance> balances = _balanceRepository.GetAll();
            List<Asset> assets = _assetRepository.GetAll();

            balances = balances.Where(b => b.EffectiveDate.Equals(effectiveDate)).ToList();

            balances.ForEach(b => b.Asset = assets.FirstOrDefault(a => b.AssetId == a.Id));

            List<BalanceViewModel> items = balances.Select(b => new BalanceViewModel(b)).ToList();

            List<ExchangeRate> rates = _exchangeRateRepository.GetByEffectiveDate(effectiveDate);

            List<ExchangeRateModel> ratesModels = rates
                .Where(r => r.BaseCurrencyName != CurrencyConstants.UAH)
                .Select(r => new ExchangeRateModel(r))
                .ToList();

            IEnumerable<BalanceTotalModel> totals = CalculateTotals(balances, effectiveDate);

            items = items.OrderBy(i => i.Asset.Name).ToList();

            return new BalancesViewModel(items, items.Count, totals, ratesModels);
        }

        public BalanceViewModel GetById(Guid id)
        {
            Balance balance = _balanceRepository.GetById(id);

            BalanceViewModel result = new BalanceViewModel(balance);

            return result;
        }

        public Balance Add(BalanceCreateModel createModel)
        {
            return _balanceRepository.Add(createModel.AssetId, createModel.EffectiveDate, createModel.Value, createModel.ExchangeRateId);
        }

        public List<BalanceViewModel> AddEffectiveBalancesTemplate()
        {
            List<DateTime> effectiveDates = _balanceRepository.GetOrderedEffectiveDates();
            DateTime today = DateTime.UtcNow.Date;
            bool todayBalancesExists = effectiveDates.Contains(today);

            List<BalanceViewModel> result = new List<BalanceViewModel>();

            if (!todayBalancesExists)
            {
                List<ExchangeRateModel> todayExchangeRates = _exchangeRateService.AddDefaultExchangeRates(today);

                if (effectiveDates.Any())
                {
                    DateTime latestEffectiveDate = effectiveDates.FirstOrDefault();
                    List<Balance> latestBalances = _balanceRepository.GetByEffectiveDate(latestEffectiveDate);

                    result = AddBalancesByTemplate(latestBalances, todayExchangeRates, today);
                }
                else
                {
                    result = AddAssetsBalances(todayExchangeRates, today);
                }
            }
            else
            {
                List<Balance> balances = _balanceRepository.GetByEffectiveDate(today);
                result = balances.Select(b => new BalanceViewModel(b)).ToList();
            }

            return result;
        }

        public Balance Update(Guid id, BalanceUpdateModel updateModel)
        {
            return _balanceRepository.Update(id, updateModel.Value);
        }

        public void Delete(Guid Id)
        {
            _balanceRepository.Remove(Id);
        }

        public IEnumerable<DateTime> GetEffectiveDates()
        {
            return _balanceRepository.GetOrderedEffectiveDates();
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

        public void ApplyExchangeRate(ExchangeRateModel model)
        {
            _exchangeRateRepository.Update(model.Id, model.Value);

            _balanceRepository.ApplyExchangeRate(model.Id, model.BaseCurrencyId, model.EffectiveDate);
        }

        public IEnumerable<BalanceTotalModel> GetCurrencyTotals(string currencyName, int count)
        {
            List<Balance> balances = _balanceRepository.GetAll();
            List<Asset> assets = _assetRepository.GetAll();
            balances.ForEach(b => b.Asset = assets.FirstOrDefault(a => b.AssetId == a.Id));

            List<DateTime> effectivateDates = _balanceRepository.GetOrderedEffectiveDates().Take(count).ToList();
            List<Currency> currencies = _currencyRepository.GetAll();
            Currency currency = currencies.FirstOrDefault(c => c.Name == currencyName);
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

            List<BalanceExportModel> items = balances
                .Select(b => new BalanceExportModel
                    (b.EffectiveDate, b.Asset.Name, b.Value, b.Asset.CurrencyName, b.ExchangeRate.Rate))
                .ToList();

            string csv = _csvExporter.ExportToCsv(items);
            byte[] bytes = Encoding.ASCII.GetBytes(csv);

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

        private List<BalanceViewModel> AddBalancesByTemplate(List<Balance> balances, List<ExchangeRateModel> exchangeRates, DateTime effectiveDate)
        {
            var result = new List<BalanceViewModel>();

            foreach (Balance balance in balances)
            {
                ExchangeRateModel balanceExchangeRate = exchangeRates.FirstOrDefault(r => r.BaseCurrencyId == balance.Asset.CurrencyId);
                Balance newBalance = _balanceRepository.Add(balance.AssetId, effectiveDate, balance.Value, balanceExchangeRate.Id);

                var assetModel = new AssetViewModel(balance.Asset);
                var model = new BalanceViewModel(newBalance, balanceExchangeRate, assetModel);
                result.Add(model);
            }

            return result;
        }

        private List<BalanceViewModel> AddAssetsBalances(List<ExchangeRateModel> exchangeRates, DateTime effectiveDate)
        {
            var result = new List<BalanceViewModel>();

            List<Asset> activeAssets = _assetRepository.GetActiveAssets();

            foreach (Asset asset in activeAssets)
            {
                ExchangeRateModel exchangeRate = exchangeRates.FirstOrDefault(r => r.BaseCurrencyId == asset.CurrencyId);
                Balance newBalance = _balanceRepository.Add(asset.Id, effectiveDate, 0.0M, exchangeRate.Id);

                var assetModel = new AssetViewModel(asset);
                var balanceModel = new BalanceViewModel(newBalance, exchangeRate, assetModel);
                result.Add(balanceModel);
            }

            return result;
        }
    }
}
