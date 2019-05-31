using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Application.BalancesTotal;
using Sho.Pocket.Application.DataExport;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Balances
{
    public class BalanceService : IBalanceService
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IBalancesTotalService _balancesTotalService;
        private readonly ICsvExporter _csvExporter;

        public BalanceService(
            IBalanceRepository balanceRepository,
            IAssetRepository assetRepository,
            IExchangeRateRepository exchangeRateRepository,
            IExchangeRateService exchangeRateService,
            IBalancesTotalService balancesTotalService,
            ICsvExporter balanceExporter)
        {
            _balanceRepository = balanceRepository;
            _assetRepository = assetRepository;
            _exchangeRateRepository = exchangeRateRepository;
            _exchangeRateService = exchangeRateService;
            _balancesTotalService = balancesTotalService;
            _csvExporter = balanceExporter;
        }

        public async Task<BalancesViewModel> GetUserLatestBalancesAsync(Guid userOpenId)
        {
            List<DateTime> effectiveDates = await GetEffectiveDatesAsync(userOpenId);
            DateTime latestDate = effectiveDates.FirstOrDefault();

            BalancesViewModel result = await GetUserEffectiveBalancesAsync(userOpenId, latestDate);

            return result;
        }

        public async Task<BalancesViewModel> GetUserEffectiveBalancesAsync(Guid userOpenId, DateTime effectiveDate)
        {
            IEnumerable<Balance> balances = await _balanceRepository.GetByEffectiveDateAsync(userOpenId, effectiveDate);

            List<BalanceViewModel> items = balances
                .Select(b => new BalanceViewModel(b))
                .OrderBy(i => i.Asset.Name)
                .ToList();

            IEnumerable<ExchangeRate> rates = await _exchangeRateRepository.GetByEffectiveDateAsync(effectiveDate);
            List<ExchangeRateModel> ratesModels = rates.Select(r => new ExchangeRateModel(r)).ToList();

            IEnumerable<BalanceTotalModel> totals = await _balancesTotalService.CalculateTotalsAsync(balances, effectiveDate);

            BalancesViewModel result = new BalancesViewModel(items, items.Count, totals, ratesModels);

            return result;
        }

        public async Task<BalanceViewModel> GetUserBalanceAsync(Guid userOpenId, Guid id)
        {
            Balance balance = await _balanceRepository.GetByIdAsync(userOpenId, id);

            BalanceViewModel result = new BalanceViewModel(balance);

            return result;
        }

        public async Task<BalanceViewModel> AddBalanceAsync(Guid userOpenId, BalanceCreateModel createModel)
        {
            Balance balance = await _balanceRepository.CreateAsync(userOpenId, createModel.AssetId, createModel.EffectiveDate, createModel.Value, createModel.ExchangeRateId);
            BalanceViewModel result = new BalanceViewModel(balance);

            return result;
        }

        public async Task<List<BalanceViewModel>> AddEffectiveBalancesTemplate(Guid userOpenId)
        {
            IEnumerable<DateTime> effectiveDates = await _balanceRepository.GetOrderedEffectiveDatesAsync(userOpenId);
            DateTime today = DateTime.UtcNow.Date;
            bool todayBalancesExists = effectiveDates.Contains(today);

            List<BalanceViewModel> result;

            if (!todayBalancesExists)
            {
                List<ExchangeRateModel> todayExchangeRates = await _exchangeRateService.AddDefaultExchangeRates(today);

                if (effectiveDates.Any())
                {
                    DateTime latestEffectiveDate = effectiveDates.FirstOrDefault();
                    IEnumerable<Balance> latestBalances = await _balanceRepository.GetByEffectiveDateAsync(userOpenId, latestEffectiveDate);

                    result = await AddBalancesByTemplateAsync(userOpenId, latestBalances, todayExchangeRates, today);
                }
                else
                {
                    result = await AddAssetsBalancesAsync(userOpenId, todayExchangeRates, today);
                }
            }
            else
            {
                IEnumerable<Balance> balances = await _balanceRepository.GetByEffectiveDateAsync(userOpenId, today);
                result = balances.Select(b => new BalanceViewModel(b)).ToList();
            }

            return result;
        }

        public async Task<BalanceViewModel> UpdateBalanceAsync(Guid userOpenId, Guid id, BalanceUpdateModel updateModel)
        {
            Balance balance= await _balanceRepository.UpdateAsync(userOpenId, id, updateModel.Value);
            BalanceViewModel result = new BalanceViewModel(balance);

            return result;
        }

        public async Task<bool> DeleteBalanceAsync(Guid userOpenId, Guid id)
        {
            bool result = await _balanceRepository.RemoveAsync(userOpenId, id);

            return result;
        }

        public async Task<List<DateTime>> GetEffectiveDatesAsync(Guid userOpenId)
        {
            IEnumerable<DateTime> result = await _balanceRepository.GetOrderedEffectiveDatesAsync(userOpenId);

            return result.ToList();
        }

        public async Task<byte[]> ExportUserBalancesToCsvAsync(Guid userOpenId)
        {
            IEnumerable<Balance> balances = await _balanceRepository.GetAllAsync(userOpenId);

            List<BalanceExportModel> items = balances
                .Select(b => new BalanceExportModel(b.EffectiveDate, b.Asset.Name, b.Value, b.Asset.Currency, b.ExchangeRate.Rate))
                .ToList();

            string csv = _csvExporter.ExportToCsv(items);
            byte[] bytes = Encoding.ASCII.GetBytes(csv);

            return bytes;
        }

        private async Task<List<BalanceViewModel>> AddBalancesByTemplateAsync(Guid userOpenId, IEnumerable<Balance> balances, IEnumerable<ExchangeRateModel> exchangeRates, DateTime effectiveDate)
        {
            List<BalanceViewModel> result = new List<BalanceViewModel>();

            foreach (Balance balance in balances)
            {
                ExchangeRateModel balanceExchangeRate = exchangeRates.FirstOrDefault(r => r.BaseCurrency == balance.Asset.Currency);
                Balance newBalance = await _balanceRepository.CreateAsync(userOpenId, balance.AssetId, effectiveDate, balance.Value, balanceExchangeRate.Id);

                var assetModel = new AssetViewModel(balance.Asset);
                var model = new BalanceViewModel(newBalance, balanceExchangeRate, assetModel);
                result.Add(model);
            }

            return result;
        }

        private async Task<List<BalanceViewModel>> AddAssetsBalancesAsync(Guid userOpenId, IEnumerable<ExchangeRateModel> exchangeRates, DateTime effectiveDate)
        {
            List<BalanceViewModel> result = new List<BalanceViewModel>();
            IEnumerable<Asset> activeAssets = await _assetRepository.GetActiveAssetsAsync();

            foreach (Asset asset in activeAssets)
            {
                ExchangeRateModel exchangeRate = exchangeRates.FirstOrDefault(r => r.BaseCurrency == asset.Currency);
                Balance newBalance = await _balanceRepository.CreateAsync(userOpenId, asset.Id, effectiveDate, 0.0M, exchangeRate.Id);

                var assetModel = new AssetViewModel(asset);
                var balanceModel = new BalanceViewModel(newBalance, exchangeRate, assetModel);
                result.Add(balanceModel);
            }

            return result;
        }
    }
}
