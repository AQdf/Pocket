using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Application.DataExport;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
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

        public async Task<BalancesViewModel> GetAll(DateTime effectiveDate)
        {
            IEnumerable<Balance> balances = await _balanceRepository.GetByEffectiveDate(effectiveDate);
            IEnumerable<Asset> assets = await _assetRepository.GetAll();

            // TODO: Improve way of populating currency name in balance's asset
            foreach (Balance balance in balances)
            {
                balance.Asset = assets.FirstOrDefault(a => a.Id == balance.AssetId);
            }

            List<BalanceViewModel> items = balances
                .Select(b => new BalanceViewModel(b))
                .OrderBy(i => i.Asset.Name)
                .ToList();

            IEnumerable<ExchangeRate> rates = await _exchangeRateRepository.GetByEffectiveDate(effectiveDate);
            List<ExchangeRateModel> ratesModels = rates.Select(r => new ExchangeRateModel(r)).ToList();

            IEnumerable<BalanceTotalModel> totals = await CalculateTotals(balances, effectiveDate);

            BalancesViewModel result = new BalancesViewModel(items, items.Count, totals, ratesModels);

            return result;
        }

        public async Task<BalanceViewModel> GetById(Guid id)
        {
            Balance balance = await _balanceRepository.GetById(id);

            BalanceViewModel result = new BalanceViewModel(balance);

            return result;
        }

        public async Task<Balance> Add(BalanceCreateModel createModel)
        {
            Balance result = await _balanceRepository.Add(createModel.AssetId, createModel.EffectiveDate, createModel.Value, createModel.ExchangeRateId);

            return result;
        }

        public async Task<IEnumerable<BalanceViewModel>> AddEffectiveBalancesTemplate()
        {
            IEnumerable<DateTime> effectiveDates = await _balanceRepository.GetOrderedEffectiveDates();
            DateTime today = DateTime.UtcNow.Date;
            bool todayBalancesExists = effectiveDates.Contains(today);

            IEnumerable<BalanceViewModel> result;

            if (!todayBalancesExists)
            {
                IEnumerable<ExchangeRateModel> todayExchangeRates = await _exchangeRateService.AddDefaultExchangeRates(today);

                if (effectiveDates.Any())
                {
                    DateTime latestEffectiveDate = effectiveDates.FirstOrDefault();
                    IEnumerable<Balance> latestBalances = await _balanceRepository.GetByEffectiveDate(latestEffectiveDate);

                    result = await AddBalancesByTemplate(latestBalances, todayExchangeRates, today);
                }
                else
                {
                    result = await AddAssetsBalances(todayExchangeRates, today);
                }
            }
            else
            {
                IEnumerable<Balance> balances = await _balanceRepository.GetByEffectiveDate(today);
                IEnumerable<Asset> assets = await _assetRepository.GetAll();

                // TODO: Improve way of populating currency name in balance's asset
                foreach (Balance balance in balances)
                {
                    balance.Asset = assets.FirstOrDefault(a => a.Id == balance.AssetId);
                }

                result = balances.Select(b => new BalanceViewModel(b));
            }

            return result;
        }

        public async Task<Balance> Update(Guid id, BalanceUpdateModel updateModel)
        {
            Balance result = await _balanceRepository.Update(id, updateModel.Value);

            return result;
        }

        public async Task Delete(Guid Id)
        {
            await _balanceRepository.Remove(Id);
        }

        public async Task<IEnumerable<DateTime>> GetEffectiveDates()
        {
            IEnumerable<DateTime> result = await _balanceRepository.GetOrderedEffectiveDates();

            return result;
        }

        public async Task<IEnumerable<BalanceTotalModel>> GetCurrentTotalBalance()
        {
            IEnumerable<DateTime> effectiveDates = await _balanceRepository.GetOrderedEffectiveDates();
            DateTime latestEffectiveDate = effectiveDates.FirstOrDefault();
            IEnumerable<Balance> balances = await _balanceRepository.GetByEffectiveDate(latestEffectiveDate);

            if (!balances.Any())
            {
                return null;
            }

            IEnumerable<BalanceTotalModel> totals = await CalculateTotals(balances, latestEffectiveDate);

            List<BalanceTotalModel> result = totals
                .Where(t => t.Currency == CurrencyConstants.UAH || t.Currency == CurrencyConstants.USD)
                .ToList();

            return result;
        }

        public async Task ApplyExchangeRate(ExchangeRateModel model)
        {
            await _exchangeRateRepository.Update(model.Id, model.Value);

            await _balanceRepository.ApplyExchangeRate(model.Id, model.BaseCurrencyId, model.EffectiveDate);
        }

        public async Task<IEnumerable<BalanceTotalModel>> GetCurrencyTotals(string currencyName, int count)
        {
            IEnumerable<DateTime> effectivateDates = await _balanceRepository.GetOrderedEffectiveDates();
            IEnumerable<DateTime> totalsEffectiveDates = effectivateDates.Take(count);

            IEnumerable<Currency> currencies = await _currencyRepository.GetAll();
            Currency currency = currencies.FirstOrDefault(c => c.Name == currencyName);
            Guid defaultCurrencyId = currencies.Where(c => c.IsDefault).Select(c => c.Id).FirstOrDefault();

            List<BalanceTotalModel> result = new List<BalanceTotalModel>();

            foreach (DateTime effectiveDate in totalsEffectiveDates)
            {
                IEnumerable<Balance> balances = await _balanceRepository.GetByEffectiveDate(effectiveDate);
                BalanceTotalModel balanceTotal = await CalculateCurrencyTotal(balances, currency, defaultCurrencyId, effectiveDate);
                result.Add(balanceTotal);
            }

            return result;
        }

        public async Task<byte[]> ExportBalancesToCsv()
        {
            IEnumerable<Balance> balances = await _balanceRepository.GetAll();

            List<BalanceExportModel> items = balances
                .Select(b => new BalanceExportModel(b.EffectiveDate, b.Asset.Name, b.Value, b.Asset.CurrencyName, b.ExchangeRate.Rate))
                .ToList();

            string csv = _csvExporter.ExportToCsv(items);
            byte[] bytes = Encoding.ASCII.GetBytes(csv);

            return bytes;
        }

        private async Task<IEnumerable<BalanceTotalModel>> CalculateTotals(IEnumerable<Balance> balances, DateTime effectiveDate)
        {
            IEnumerable<Currency> currencies = await _currencyRepository.GetAll();
            Guid defaultCurrencyId = currencies.Where(c => c.IsDefault).Select(c => c.Id).FirstOrDefault();

            IEnumerable<Task<BalanceTotalModel>> totalsCalculationTasks = currencies
                .Select(c => CalculateCurrencyTotal(balances, c, defaultCurrencyId, effectiveDate));

            BalanceTotalModel[] balanceTotals = await Task.WhenAll(totalsCalculationTasks);
            List<BalanceTotalModel> result = balanceTotals.OrderBy(c => c.Currency).ToList();

            return result;
        }

        private async Task<BalanceTotalModel> CalculateCurrencyTotal(IEnumerable<Balance> balances, Currency currency, Guid defaultCurrencyId, DateTime effectiveDate)
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

                ExchangeRate currentCurrencyExchangeRate = await _exchangeRateRepository.GetCurrencyExchangeRate(currency.Id, effectiveDate);

                value = currentCurrencyBalances.Select(b => b.Value).Sum()
                    + defaultCurrencyBalances.Select(b => b.Value / currentCurrencyExchangeRate.Rate).Sum()
                    + otherCurrenciesBalances.Select(b => b.Value * b.ExchangeRate.Rate / currentCurrencyExchangeRate.Rate).Sum();
            }

            BalanceTotalModel result = new BalanceTotalModel(effectiveDate, currency.Name, value);

            return result;
        }

        private async Task<List<BalanceViewModel>> AddBalancesByTemplate(IEnumerable<Balance> balances, IEnumerable<ExchangeRateModel> exchangeRates, DateTime effectiveDate)
        {
            List<BalanceViewModel> result = new List<BalanceViewModel>();

            foreach (Balance balance in balances)
            {
                ExchangeRateModel balanceExchangeRate = exchangeRates.FirstOrDefault(r => r.BaseCurrencyId == balance.Asset.CurrencyId);
                Balance newBalance = await _balanceRepository.Add(balance.AssetId, effectiveDate, balance.Value, balanceExchangeRate.Id);

                var assetModel = new AssetViewModel(balance.Asset);
                var model = new BalanceViewModel(newBalance, balanceExchangeRate, assetModel);
                result.Add(model);
            }

            return result;
        }

        private async Task<List<BalanceViewModel>> AddAssetsBalances(IEnumerable<ExchangeRateModel> exchangeRates, DateTime effectiveDate)
        {
            List<BalanceViewModel> result = new List<BalanceViewModel>();
            IEnumerable<Asset> activeAssets = await _assetRepository.GetActiveAssets();

            foreach (Asset asset in activeAssets)
            {
                ExchangeRateModel exchangeRate = exchangeRates.FirstOrDefault(r => r.BaseCurrencyId == asset.CurrencyId);
                Balance newBalance = await _balanceRepository.Add(asset.Id, effectiveDate, 0.0M, exchangeRate.Id);

                var assetModel = new AssetViewModel(asset);
                var balanceModel = new BalanceViewModel(newBalance, exchangeRate, assetModel);
                result.Add(balanceModel);
            }

            return result;
        }
    }
}
