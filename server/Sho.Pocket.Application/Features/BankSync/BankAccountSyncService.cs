using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.BankSync.Abstractions;
using Sho.Pocket.Core.Features.BankSync.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Features.BankSync
{
    public class BankAccountSyncService : IBankAccountSyncService
    {
        private readonly IBankAccountServiceResolver _bankAccountServiceResolver;

        private readonly IAssetBankAccountRepository _assetBankAccountRepository;

        private readonly IBankRepository _bankRepository;

        private readonly IMemoryCache _cache;

        public BankAccountSyncService(
            IBankAccountServiceResolver bankAccountServiceResolver,
            IAssetBankAccountRepository assetBankAccountRepository,
            IBankRepository bankRepository,
            IMemoryCache cache)
        {
            _bankAccountServiceResolver = bankAccountServiceResolver;
            _assetBankAccountRepository = assetBankAccountRepository;
            _bankRepository = bankRepository;
            _cache = cache;
        }

        public async Task<AssetBankAccountViewModel> GetAssetBankAccountAsync(Guid userId, Guid assetId)
        {
            AssetBankAccount assetBankAccount = await _assetBankAccountRepository.GetAsync(userId, assetId);
            AssetBankAccountViewModel result = assetBankAccount != null 
                ? new AssetBankAccountViewModel(assetBankAccount) 
                : null;

            return result;
        }

        public async Task<List<string>> GetSupportedBanksAsync()
        {
            IEnumerable<Bank> banks = await _bankRepository.GetSupportedBanksAsync();
            List<string> result = banks.Select(b => b.Name).ToList();

            return result;
        }

        public async Task<List<BankAccount>> SubmitBankClientAuthDataAsync(Guid userId, Guid assetId, string bankName, string token, string bankClientId, string cardNumber)
        {
            //TODO: Catch errors. Verify if token is correct and then save it to database. Encrypt this token.
            BankAccountsRequestParams clientData = new BankAccountsRequestParams(token, bankClientId, cardNumber);
            IBankAccountService bankAccountservice = _bankAccountServiceResolver.Resolve(bankName);
            IReadOnlyCollection<BankAccountBalance> accountBalances = await bankAccountservice.GetAccountsAsync(clientData);

            if (accountBalances == null)
            {
                throw new Exception($"Sync with bank {bankName} failed.");
            }

            AssetBankAccount bankAccount = await _assetBankAccountRepository.AlterAsync(userId, assetId, bankName, token, bankClientId);
            List<BankAccount> result = accountBalances.Select(a => new BankAccount(a.AccountId, a.AccountName)).ToList();

            return result;
        }

        public async Task<bool> ConnectAssetWithBankAcountAsync(Guid userId, Guid assetId, string bankName, string accountName, string bankAccountId)
        {
            await _assetBankAccountRepository.UpdateAccountAsync(userId, assetId, accountName, bankAccountId);

            return true;
        }

        public async Task<bool> DisconnectAssetWithBankAcountAsync(Guid userId, Guid assetId)
        {
            await _assetBankAccountRepository.DeleteAsync(userId, assetId);

            return true;
        }

        public async Task<BankAccountBalance> GetBankAccountBalanceAsync(Guid userId, Guid assetId)
        {
            AssetBankAccount account = await _assetBankAccountRepository.GetAsync(userId, assetId);

            if (account == null)
            {
                throw new Exception("Asset bank account not found.");
            }

            Bank bank = await _bankRepository.GetBankAsync(account.BankName);
            TimeSpan syncSpan = DateTime.UtcNow - account.LastSyncDateTime;
            BankAccountBalance accountBalance = null;

            if (syncSpan.TotalSeconds > bank.SyncFreqInSeconds)
            {
                IBankAccountService bankAccountservice = _bankAccountServiceResolver.Resolve(account.BankName);
                var requestParams = new BankAccountsRequestParams(account.Token, account.BankClientId, account.BankAccountId);
                IReadOnlyCollection<BankAccountBalance> accountBalances = await bankAccountservice.GetAccountsAsync(requestParams);
                accountBalance = accountBalances.FirstOrDefault(ab => ab.AccountId == account.BankAccountId);

                await _assetBankAccountRepository.UpdateLastSyncAsync(userId, assetId, DateTime.UtcNow, accountBalance.AccountName);
            }

            return accountBalance;
        }

        public async Task<List<AssetTransactionViewModel>> GetAssetBankAccountTransactionsAsync(Guid userId, Guid assetId)
        {
            string cacheKey = $"{assetId}_{nameof(AssetTransactionViewModel)}s";
            AssetBankAccount account = await _assetBankAccountRepository.GetAsync(userId, assetId);

            if (account == null)
            {
                throw new Exception("Asset bank account not found.");
            }

            if (_cache.TryGetValue(cacheKey, out List<AssetTransactionViewModel> transactions))
            {
                return transactions;
            }

            IBankAccountService bankAccountservice = _bankAccountServiceResolver.Resolve(account.BankName);
            // TODO: Remove hard-coded dates.
            DateTime now = DateTime.UtcNow;
            DateTime from = new DateTime(now.Year, now.Month, 1);
            var requestParams = new AccountStatementRequestParams(account.Token, account.BankAccountId, account.BankClientId, from, now);
            var accountTransactions = await bankAccountservice.GetAccountTransactionsAsync(requestParams);

            transactions = accountTransactions
                .Select(t => new AssetTransactionViewModel(assetId, t.TransactionDate, t.Description, t.Currency, t.Amount, t.Balance))
                .ToList();

            _cache.Set(cacheKey, transactions, TimeSpan.FromMinutes(5));

            return transactions;
        }
    }
}
