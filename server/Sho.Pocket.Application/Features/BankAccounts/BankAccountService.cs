using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Sho.Pocket.BankIntegration.Abstractions;
using Sho.Pocket.BankIntegration.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.BankAccounts;
using Sho.Pocket.Core.Features.BankAccounts.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Features.BankAccounts
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankService _bankService;

        private readonly IBankIntegrationServiceResolver _bankIntegrationServiceResolver;

        private readonly IBankAccountRepository _bankAccountRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMemoryCache _cache;

        public BankAccountService(
            IBankIntegrationServiceResolver bankIntegrationServiceResolver,
            IBankService bankService,
            IBankAccountRepository assetBankAccountRepository,
            IUnitOfWork unitOfWork,
            IMemoryCache cache)
        {
            _bankService = bankService;
            _bankIntegrationServiceResolver = bankIntegrationServiceResolver;
            _bankAccountRepository = assetBankAccountRepository;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<BankAccountModel> GetBankAccountAsync(Guid userId, Guid assetId)
        {
            AssetBankAccount assetBankAccount = await _bankAccountRepository.GetByAssetIdAsync(userId, assetId);
            BankAccountModel result = assetBankAccount != null 
                ? new BankAccountModel(assetBankAccount) 
                : null;

            return result;
        }

        public List<string> GetSupportedBanks()
        {
            IReadOnlyCollection<BankModel> banks = _bankService.GetBanks();
            List<string> result = banks.Select(b => b.Name).ToList();

            return result;
        }

        public async Task<List<ExternalBankAccountModel>> SubmitBankClientAuthDataAsync(Guid userId, Guid assetId, string bankName, string token, string bankClientId, string cardNumber)
        {
            //TODO: Catch errors. Verify if token is correct and then save it to database. Encrypt this token.
            BankAccountsRequestParams clientData = new BankAccountsRequestParams(token, bankClientId, cardNumber);
            IBankIntegrationService bankIntegrationService = _bankIntegrationServiceResolver.Resolve(bankName);
            IReadOnlyCollection<ExternalAccountBalanceModel> accountBalances = await bankIntegrationService.GetAccountsAsync(clientData);

            if (accountBalances == null)
            {
                throw new Exception($"Sync with bank {bankName} failed.");
            }

            AssetBankAccount bankAccount = await _bankAccountRepository.AlterAsync(userId, assetId, bankName, token, bankClientId);
            await _unitOfWork.SaveChangesAsync();

            List<ExternalBankAccountModel> result = accountBalances
                .Select(a => new ExternalBankAccountModel(a.AccountId, a.AccountName))
                .ToList();

            return result;
        }

        public async Task<bool> ConnectAssetWithBankAcountAsync
            (Guid userId, Guid assetId, string bankName, string accountName, string bankAccountId)
        {
            await _bankAccountRepository.UpdateAccountAsync(userId, assetId, accountName, bankAccountId);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DisconnectAssetWithBankAcountAsync(Guid userId, Guid assetId)
        {
            await _bankAccountRepository.DeleteAsync(userId, assetId);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<BankAccountBalance> GetBankAccountBalanceAsync(Guid userId, Guid assetId)
        {
            AssetBankAccount account = await _bankAccountRepository.GetByAssetIdAsync(userId, assetId);

            if (account == null)
            {
                throw new Exception("Asset bank account not found.");
            }

            BankModel bank = _bankService.GetBank(account.BankName);
            TimeSpan syncSpan = DateTime.UtcNow - account.LastSyncDateTime;
            BankAccountBalance accountBalance = null;

            if (syncSpan.TotalSeconds > bank.SyncFreqInSeconds)
            {
                IBankIntegrationService bankAccountservice = _bankIntegrationServiceResolver.Resolve(account.BankName);
                var requestParams = new BankAccountsRequestParams(account.Token, account.BankClientId, account.BankAccountId);
                IReadOnlyCollection<ExternalAccountBalanceModel> accountBalances = await bankAccountservice.GetAccountsAsync(requestParams);
                ExternalAccountBalanceModel external = accountBalances.FirstOrDefault(ab => ab.AccountId == account.BankAccountId);
                accountBalance = new BankAccountBalance(external.BankName, external.AccountId, external.Currency, external.Balance);

                await _bankAccountRepository.UpdateLastSyncAsync(userId, assetId, DateTime.UtcNow, accountBalance.AccountName);
                await _unitOfWork.SaveChangesAsync();
            }

            return accountBalance;
        }

        public async Task<List<BankAccountTransactionModel>> GetBankAccountTransactionsAsync(Guid userId, Guid assetId)
        {
            string cacheKey = $"{assetId}_{nameof(BankAccountTransactionModel)}s";
            AssetBankAccount account = await _bankAccountRepository.GetByAssetIdAsync(userId, assetId);

            if (account == null)
            {
                throw new Exception("Asset bank account not found.");
            }

            if (_cache.TryGetValue(cacheKey, out List<BankAccountTransactionModel> transactions))
            {
                return transactions;
            }

            IBankIntegrationService bankAccountservice = _bankIntegrationServiceResolver.Resolve(account.BankName);

            // TODO: Remove hard-coded dates.
            DateTime now = DateTime.UtcNow;
            DateTime from = new DateTime(now.Year, now.Month, 1);
            var requestParams = new AccountStatementRequestParams(account.Token, account.BankAccountId, account.BankClientId, from, now);
            var accountTransactions = await bankAccountservice.GetAccountTransactionsAsync(requestParams);

            transactions = accountTransactions
                .Select(t => new BankAccountTransactionModel(assetId, t.TransactionDate, t.Description, t.Currency, t.Amount, t.Balance))
                .ToList();

            _cache.Set(cacheKey, transactions, TimeSpan.FromMinutes(5));

            return transactions;
        }
    }
}
