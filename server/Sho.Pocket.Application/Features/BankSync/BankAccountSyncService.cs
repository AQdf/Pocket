using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public BankAccountSyncService(
            IBankAccountServiceResolver bankAccountServiceResolver,
            IAssetBankAccountRepository assetBankAccountRepository,
            IBankRepository bankRepository)
        {
            _bankAccountServiceResolver = bankAccountServiceResolver;
            _assetBankAccountRepository = assetBankAccountRepository;
            _bankRepository = bankRepository;
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
            BankClientData clientData = new BankClientData(token, bankClientId, cardNumber);
            IBankAccountService bankAccountservice = _bankAccountServiceResolver.Resolve(bankName);
            IReadOnlyCollection<BankAccountBalance> accountBalances = await bankAccountservice.GetClientAccountsInfoAsync(clientData);

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
            AssetBankAccount assetBankAccount = await _assetBankAccountRepository.GetAsync(userId, assetId);

            if (assetBankAccount == null)
            {
                throw new Exception("Bank account not found.");
            }

            Bank bank = await _bankRepository.GetBankAsync(assetBankAccount.BankName);
            TimeSpan syncSpan = DateTime.UtcNow - assetBankAccount.LastSyncDateTime;
            BankAccountBalance accountBalance = null;

            if (syncSpan.TotalSeconds > bank.SyncFreqInSeconds)
            {
                IBankAccountService bankAccountservice = _bankAccountServiceResolver.Resolve(assetBankAccount.BankName);
                BankClientData clientData = new BankClientData(assetBankAccount.Token, assetBankAccount.BankClientId, assetBankAccount.BankAccountId);
                IReadOnlyCollection<BankAccountBalance> accountBalances = await bankAccountservice.GetClientAccountsInfoAsync(clientData);
                accountBalance = accountBalances.FirstOrDefault(ab => ab.AccountId == assetBankAccount.BankAccountId);

                await _assetBankAccountRepository.UpdateLastSyncAsync(userId, assetId, DateTime.UtcNow, accountBalance.AccountName);
            }

            return accountBalance;
        }
    }
}
