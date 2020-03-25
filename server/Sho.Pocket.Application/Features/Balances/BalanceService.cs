using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.Application.Exceptions;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.Assets.Models;
using Sho.Pocket.Core.Features.Balances.Abstractions;
using Sho.Pocket.Core.Features.Balances.Models;
using Sho.Pocket.Core.Features.BankAccounts;
using Sho.Pocket.Core.Features.BankAccounts.Models;
using Sho.Pocket.Core.Features.ExchangeRates;
using Sho.Pocket.Core.Features.ExchangeRates.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Balances
{
    public class BalanceService : IBalanceService
    {
        private readonly IBalanceRepository _balanceRepository;

        private readonly IAssetRepository _assetRepository;

        private readonly IBankAccountRepository _assetBankAccountRepository;

        private readonly IExchangeRateService _exchangeRateService;

        private readonly IBalancesTotalService _balancesTotalService;

        private readonly IBankAccountService _bankAccountService;

        private readonly IUnitOfWork _unitOfWork;

        public BalanceService(
            IBalanceRepository balanceRepository,
            IAssetRepository assetRepository,
            IBankAccountRepository assetBankAccountRepository,
            IExchangeRateService exchangeRateService,
            IBalancesTotalService balancesTotalService,
            IBankAccountService bankAccountService,
            IUnitOfWork unitOfWork)
        {
            _balanceRepository = balanceRepository;
            _assetRepository = assetRepository;
            _assetBankAccountRepository = assetBankAccountRepository;
            _exchangeRateService = exchangeRateService;
            _balancesTotalService = balancesTotalService;
            _bankAccountService = bankAccountService;
            _unitOfWork = unitOfWork;
        }

        public async Task<BalancesViewModel> GetUserEffectiveBalancesAsync(Guid userId, DateTime effectiveDate)
        {
            IEnumerable<Balance> balances = await _balanceRepository.GetByEffectiveDateAsync(userId, effectiveDate);

            List<BalanceViewModel> items = balances
                .Select(b => new BalanceViewModel(b))
                .OrderBy(i => i.Asset.Currency)
                .ThenBy(i => i.Asset.Name)
                .ToList();

            if (effectiveDate == DateTime.UtcNow.Date)
            {
                await PopulateIsBankAccountFieldAsync(userId, items);
            }

            IEnumerable<BalanceTotalModel> totals = await _balancesTotalService.CalculateTotalsAsync(userId, balances, effectiveDate);

            BalancesViewModel result = new BalancesViewModel(items, items.Count, totals);

            return result;
        }

        public async Task<BalanceViewModel> GetUserBalanceAsync(Guid userId, Guid id)
        {
            Balance balance = await _balanceRepository.GetByIdAsync(userId, id);

            BalanceViewModel result = new BalanceViewModel(balance);

            return result;
        }

        public async Task<BalanceViewModel> AddBalanceAsync(Guid userId, BalanceCreateModel createModel)
        {
            Balance balance = await _balanceRepository.CreateAsync(userId, createModel.AssetId, createModel.EffectiveDate, createModel.Value);
            await _unitOfWork.SaveChangesAsync();

            BalanceViewModel result = new BalanceViewModel(balance);

            return result;
        }

        public async Task<List<BalanceViewModel>> AddEffectiveBalancesTemplate(Guid userId)
        {
            IEnumerable<DateTime> effectiveDates = await _balanceRepository.GetOrderedEffectiveDatesAsync(userId);
            DateTime today = DateTime.UtcNow.Date;
            bool todayBalancesExists = effectiveDates.Contains(today);

            List<BalanceViewModel> result;

            if (!todayBalancesExists)
            {
                List<ExchangeRateModel> rates = await _exchangeRateService.AddExchangeRatesAsync(userId, today);

                if (effectiveDates.Any())
                {
                    DateTime latestEffectiveDate = effectiveDates.FirstOrDefault();
                    result = await AddBalancesByTemplateAsync(userId, latestEffectiveDate, today);
                }
                else
                {
                    result = await AddAssetsBalancesAsync(userId, today);
                }
            }
            else
            {
                IEnumerable<Balance> balances = await _balanceRepository.GetByEffectiveDateAsync(userId, today);
                result = balances.Select(b => new BalanceViewModel(b)).ToList();
            }

            return result;
        }

        public async Task<BalanceViewModel> UpdateBalanceAsync(Guid userId, Guid id, BalanceUpdateModel updateModel)
        {
            Balance balance= await _balanceRepository.UpdateAsync(userId, id, updateModel.AssetId, updateModel.Value);
            await _unitOfWork.SaveChangesAsync();

            BalanceViewModel result = new BalanceViewModel(balance);

            return result;
        }

        public async Task<bool> DeleteBalanceAsync(Guid userId, Guid id)
        {
            bool result = await _balanceRepository.RemoveAsync(userId, id);
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<List<DateTime>> GetEffectiveDatesAsync(Guid userId)
        {
            IEnumerable<DateTime> result = await _balanceRepository.GetOrderedEffectiveDatesAsync(userId);

            return result.ToList();
        }

        public async Task<BalanceViewModel> SyncBankAccountBalanceAsync(Guid userId, Guid id)
        {
            BalanceViewModel balanceViewModel;
            Balance balance = await _balanceRepository.GetByIdAsync(userId, id);

            BankAccountBalance bankAccountBalance = await _bankAccountService.GetBankAccountBalanceAsync(userId, balance.AssetId);

            if (bankAccountBalance != null)
            {
                Balance updatedBalance = await _balanceRepository.UpdateAsync(userId, id, balance.AssetId, bankAccountBalance.Balance);
                balanceViewModel = new BalanceViewModel(updatedBalance);
            }
            else
            {
                balanceViewModel = new BalanceViewModel(balance);
            }

            await _unitOfWork.SaveChangesAsync();

            return balanceViewModel;
        }

        private async Task<List<BalanceViewModel>> AddBalancesByTemplateAsync(Guid userId, DateTime latestEffectiveDate, DateTime effectiveDate)
        {
            IEnumerable<Balance> latestBalances = await _balanceRepository.GetByEffectiveDateAsync(userId, latestEffectiveDate);
            IList<AssetBankAccount> bankAccounts = await _assetBankAccountRepository.GetByUserIdAsync(userId);
            List<BalanceViewModel> result = new List<BalanceViewModel>();

            foreach (Balance balance in latestBalances)
            {
                decimal value = balance.Value;

                if (bankAccounts.Any(ba => ba.AssetId == balance.AssetId))
                {
                    BankAccountBalance bankAccountBalance = await _bankAccountService.GetBankAccountBalanceAsync(userId, balance.AssetId);

                    if (bankAccountBalance != null)
                    {
                        value = bankAccountBalance.Balance;
                    }
                }

                Balance newBalance = await _balanceRepository.CreateAsync(userId, balance.AssetId, effectiveDate, value);

                var assetModel = new AssetViewModel(balance.Asset);
                var model = new BalanceViewModel(newBalance, assetModel);
                result.Add(model);
            }

            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        private async Task<List<BalanceViewModel>> AddAssetsBalancesAsync(Guid userId, DateTime effectiveDate)
        {
            List<BalanceViewModel> result = new List<BalanceViewModel>();
            IEnumerable<Asset> activeAssets = await _assetRepository.GetByUserIdAsync(userId, false);

            if (!activeAssets.Any())
            {
                throw new UserHasNoAssetsException();
            }

            foreach (Asset asset in activeAssets)
            {
                Balance newBalance = await _balanceRepository.CreateAsync(userId, asset.Id, effectiveDate, 0.0M);

                var assetModel = new AssetViewModel(asset);
                var balanceModel = new BalanceViewModel(newBalance, assetModel);
                result.Add(balanceModel);
            }

            await _unitOfWork.SaveChangesAsync();

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
