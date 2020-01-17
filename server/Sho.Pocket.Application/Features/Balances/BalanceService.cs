using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Application.Exceptions;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.Utils.Csv;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.Assets.Models;
using Sho.Pocket.Core.Features.BankAccounts.Abstractions;
using Sho.Pocket.Core.Features.BankAccounts.Models;
using Sho.Pocket.Core.Features.ExchangeRates.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Balances
{
    public class BalanceService : IBalanceService
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IAssetBankAccountRepository _assetBankAccountRepository;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IBalancesTotalService _balancesTotalService;
        private readonly IBankAccountSyncService _accountBankSyncService;
        private readonly ICsvExporter _csvExporter;

        public BalanceService(
            IBalanceRepository balanceRepository,
            IAssetRepository assetRepository,
            IAssetBankAccountRepository assetBankAccountRepository,
            IExchangeRateRepository exchangeRateRepository,
            IExchangeRateService exchangeRateService,
            IBalancesTotalService balancesTotalService,
            IBankAccountSyncService accountBankSyncService,
            ICsvExporter balanceExporter)
        {
            _balanceRepository = balanceRepository;
            _assetRepository = assetRepository;
            _assetBankAccountRepository = assetBankAccountRepository;
            _exchangeRateRepository = exchangeRateRepository;
            _exchangeRateService = exchangeRateService;
            _balancesTotalService = balancesTotalService;
            _accountBankSyncService = accountBankSyncService;
            _csvExporter = balanceExporter;
        }

        public async Task<BalancesViewModel> GetUserLatestBalancesAsync(Guid userOpenId)
        {
            BalancesViewModel result = null;
            List<DateTime> effectiveDates = await GetEffectiveDatesAsync(userOpenId);

            if (effectiveDates != null && effectiveDates.Any())
            {
                DateTime latestDate = effectiveDates.FirstOrDefault();
                result = await GetUserEffectiveBalancesAsync(userOpenId, latestDate);
            }

            return result;
        }

        public async Task<BalancesViewModel> GetUserEffectiveBalancesAsync(Guid userOpenId, DateTime effectiveDate)
        {
            IEnumerable<Balance> balances = await _balanceRepository.GetByEffectiveDateAsync(userOpenId, effectiveDate);

            List<BalanceViewModel> items = balances
                .Select(b => new BalanceViewModel(b))
                .OrderBy(i => i.Asset.Currency)
                .ToList();

            if (effectiveDate == DateTime.UtcNow.Date)
            {
                await PopulateIsBankAccountFieldAsync(userOpenId, items);
            }

            IEnumerable<ExchangeRate> rates = await _exchangeRateRepository.GetByEffectiveDateAsync(effectiveDate);
            List<ExchangeRateModel> ratesModels = rates.Select(r => new ExchangeRateModel(r)).ToList();

            IEnumerable<BalanceTotalModel> totals = await _balancesTotalService.CalculateTotalsAsync(userOpenId, balances, effectiveDate);

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
                if (effectiveDates.Any())
                {
                    DateTime latestEffectiveDate = effectiveDates.FirstOrDefault();
                    result = await AddBalancesByTemplateAsync(userOpenId, latestEffectiveDate, today);
                }
                else
                {
                    result = await AddAssetsBalancesAsync(userOpenId, today);
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
            Balance balance= await _balanceRepository.UpdateAsync(userOpenId, id, updateModel.AssetId, updateModel.Value);
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
                .Select(b => new BalanceExportModel(b.EffectiveDate, b.Asset.Name, b.Value, b.Asset.Currency, b.ExchangeRate.Rate, b.ExchangeRate.CounterCurrency))
                .ToList();

            string csv = _csvExporter.ExportToCsv(items);
            byte[] bytes = Encoding.Default.GetBytes(csv);

            return bytes;
        }

        public async Task<byte[]> ExportJsonAsync(Guid userOpenId, DateTime effectiveDate)
        {
            IEnumerable<Balance> balances = await _balanceRepository.GetByEffectiveDateAsync(userOpenId, effectiveDate);

            List<BalanceExportModel> items = balances
                .Select(b => new BalanceExportModel(b.EffectiveDate, b.Asset.Name, b.Value, b.Asset.Currency, b.ExchangeRate.Rate, b.ExchangeRate.CounterCurrency))
                .ToList();

            string csv = JsonConvert.SerializeObject(items);
            byte[] bytes = Encoding.Default.GetBytes(csv);

            return bytes;
        }

        public async Task ImportJsonAsync(Guid userOpenId, string jsonData)
        {
            List<BalanceExportModel> items = JsonConvert.DeserializeObject<List<BalanceExportModel>>(jsonData);
            IEnumerable<IGrouping<DateTime, BalanceExportModel>> efeectiveDateGroup = items.GroupBy(i => i.EffectiveDate);

            foreach (IGrouping<DateTime, BalanceExportModel> group in efeectiveDateGroup)
            {
                bool exists = await _balanceRepository.ExistsEffectiveDateBalancesAsync(userOpenId, group.Key);

                if (!exists)
                {
                    foreach (BalanceExportModel item in group)
                    {
                        Asset asset = await _assetRepository.GetByNameAsync(userOpenId, item.Asset);

                        if (asset != null)
                        {
                            ExchangeRate rate = await _exchangeRateRepository.AlterAsync(item.EffectiveDate, item.Currency, item.CounterCurrency, item.ExchangeRate);
                            Balance balance = await _balanceRepository.CreateAsync(userOpenId, asset.Id, item.EffectiveDate, item.Value, rate.Id);
                        }
                    }
                }
            }
        }

        public async Task<BalanceViewModel> SyncBankAccountBalanceAsync(Guid userOpenId, Guid id)
        {
            BalanceViewModel balanceViewModel;
            Balance balance = await _balanceRepository.GetByIdAsync(userOpenId, id);

            BankAccountBalance bankAccountBalance = await _accountBankSyncService.GetBankAccountBalanceAsync(userOpenId, balance.AssetId);

            if (bankAccountBalance != null)
            {
                Balance updatedBalance = await _balanceRepository.UpdateAsync(userOpenId, id, balance.AssetId, bankAccountBalance.Balance);
                balanceViewModel = new BalanceViewModel(updatedBalance);
            }
            else
            {
                balanceViewModel = new BalanceViewModel(balance);
            }

            return balanceViewModel;
        }

        private async Task<List<BalanceViewModel>> AddBalancesByTemplateAsync(Guid userOpenId, DateTime latestEffectiveDate, DateTime effectiveDate)
        {
            IEnumerable<Balance> latestBalances = await _balanceRepository.GetByEffectiveDateAsync(userOpenId, latestEffectiveDate);
            List<ExchangeRateModel> exchangeRates = await _exchangeRateService.AddDefaultExchangeRates(userOpenId, effectiveDate);

            IList<AssetBankAccount> bankAccounts = await _assetBankAccountRepository.GetByUserIdAsync(userOpenId);
            List<BalanceViewModel> result = new List<BalanceViewModel>();

            foreach (Balance balance in latestBalances)
            {
                ExchangeRateModel balanceExchangeRate = exchangeRates.FirstOrDefault(r => r.BaseCurrency == balance.Asset.Currency);

                decimal value = balance.Value;

                if (bankAccounts.Any(ba => ba.AssetId == balance.AssetId))
                {
                    BankAccountBalance bankAccountBalance = await _accountBankSyncService.GetBankAccountBalanceAsync(userOpenId, balance.AssetId);

                    if (bankAccountBalance != null)
                    {
                        value = bankAccountBalance.Balance;
                    }
                }

                Balance newBalance = await _balanceRepository.CreateAsync(userOpenId, balance.AssetId, effectiveDate, value, balanceExchangeRate.Id);

                var assetModel = new AssetViewModel(balance.Asset);
                var model = new BalanceViewModel(newBalance, balanceExchangeRate, assetModel);
                result.Add(model);
            }

            return result;
        }

        private async Task<List<BalanceViewModel>> AddAssetsBalancesAsync(Guid userOpenId, DateTime effectiveDate)
        {
            List<BalanceViewModel> result = new List<BalanceViewModel>();
            IEnumerable<Asset> activeAssets = await _assetRepository.GetActiveAssetsAsync();

            if (!activeAssets.Any())
            {
                throw new UserHasNoAssetsException();
            }

            List<ExchangeRateModel> exchangeRates = await _exchangeRateService.AddDefaultExchangeRates(userOpenId, effectiveDate);

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

        private async Task PopulateIsBankAccountFieldAsync(Guid userId, List<BalanceViewModel> items)
        {
            IList<AssetBankAccount> bankAccounts = await _assetBankAccountRepository.GetByUserIdAsync(userId);

            foreach (AssetBankAccount bankAccount in bankAccounts)
            {
                BalanceViewModel balance = items.FirstOrDefault(i => i.AssetId == bankAccount.AssetId);

                if (balance != null)
                {
                    balance.IsBankAccount = true;
                }
            }
        }
    }
}
