using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.Core.BankIntegration;
using Sho.Pocket.Core.BankIntegration.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.Accounts.Models;
using Sho.Pocket.Core.Features.BankAccounts.Abstractions;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Features.BankSync
{
    public class AccountBankSyncService : IAccountBankSyncService
    {
        private readonly IBankAccountServiceResolver _bankAccountServiceResolver;

        private readonly IBankRepository _bankRepository;

        private readonly IUserBankAuthDataRepository _userBankAuthDataRepository;

        private readonly IAssetBankAccountRepository _assetBankAccountRepository;

        public AccountBankSyncService(
            IBankAccountServiceResolver bankAccountServiceResolver,
            IBankRepository bankRepository,
            IUserBankAuthDataRepository userBankAuthDataRepository,
            IAssetBankAccountRepository assetBankAccountRepository)
        {
            _bankAccountServiceResolver = bankAccountServiceResolver;
            _bankRepository = bankRepository;
            _userBankAuthDataRepository = userBankAuthDataRepository;
            _assetBankAccountRepository = assetBankAccountRepository;
        }

        public async Task<AssetBankSyncData> GetAssetBankAccountAsync(Guid userId, Guid assetId)
        {
            AssetBankAccount assetBankAccount = await _assetBankAccountRepository.GetAsync(userId, assetId);
            AssetBankSyncData result = null;

            if (assetBankAccount != null)
            {
                UserBankAuthData authData = await _userBankAuthDataRepository.GetAsync(userId, assetBankAccount.UserBankAuthDataId);

                result = new AssetBankSyncData
                {
                    AssetId = assetId,
                    BankAccountName = assetBankAccount.BankAccountName,
                    BankName = assetBankAccount.BankName,
                    TokenMask = GetMaskedToken(authData.Token)
                };
            }

            return result;
        }

        public async Task<List<string>> GetSupportedBanksAsync()
        {
            IEnumerable<Bank> banks = await _bankRepository.GetSupportedBanksAsync();
            List<string> result = banks.Select(b => b.Name).ToList();

            return result;
        }

        public async Task<List<BankAccount>> SubmitBankClientAuthDataAsync(Guid userId, string bankName, string token, string bankClientId, string cardNumber)
        {
            //TODO: Catch errors. Verify if token is correct and then save it to database. Encrypt this token.
            BankClientData clientData = new BankClientData(token, bankClientId, cardNumber);
            IBankAccountService bankAccountservice = _bankAccountServiceResolver.Resolve(bankName);
            List<BankAccountBalance> accountBalances = await bankAccountservice.GetClientAccountsInfoAsync(clientData);
            List<BankAccount> result = new List<BankAccount>();

            if (accountBalances != null)
            {
                UserBankAuthData authData = await _userBankAuthDataRepository.AlterAsync(userId, bankName, token, bankClientId);
                result = accountBalances.Select(a => new BankAccount(authData.Id, a.AccountId, a.AccountName)).ToList();
            }
            else
            {
                throw new Exception($"Sync with bank {bankName} failed.");
            }

            return result;
        }

        public async Task<bool> ConnectAssetWithBankAcountAsync(Guid userId, Guid assetId, string bankName, string accountName, string bankAccountId)
        {
            AssetBankAccount existingAccount = await _assetBankAccountRepository.GetAsync(userId, assetId);

            if (existingAccount != null)
            {
                await _assetBankAccountRepository.DeleteAsync(userId, assetId);
            }

            await _assetBankAccountRepository.CreateAsync(userId, assetId, bankName, accountName, bankAccountId);

            return true;
        }

        public async Task<bool> DisconnectAssetWithBankAcountAsync(Guid userId, Guid assetId)
        {
            await _assetBankAccountRepository.DeleteAsync(userId, assetId);

            return true;
        }

        public async Task<BankAccountBalance> GetBankAccountBalanceAsync(Guid userId, Guid assetId)
        {
            try
            {
                BankAccountBalance accountBalance = null;
                AssetBankAccount assetBankAccount = await _assetBankAccountRepository.GetAsync(userId, assetId);
                UserBankAuthData authData = await _userBankAuthDataRepository.GetAsync(userId, assetBankAccount.UserBankAuthDataId);
                Bank bank = await _bankRepository.GetBankAsync(authData.BankName);
                TimeSpan syncSpan = DateTime.UtcNow - assetBankAccount.LastSyncDateTime;

                if (syncSpan.TotalSeconds > bank.SyncFreqInSeconds)
                {
                    IBankAccountService bankAccountservice = _bankAccountServiceResolver.Resolve(authData.BankName);
                    BankClientData clientData = new BankClientData(authData.Token, authData.BankClientId, assetBankAccount.BankAccountId);
                    List<BankAccountBalance> accountBalances = await bankAccountservice.GetClientAccountsInfoAsync(clientData);
                    accountBalance = accountBalances.FirstOrDefault(ab => ab.AccountId == assetBankAccount.BankAccountId);

                    await _assetBankAccountRepository.UpdateAsync(userId, assetId, DateTime.UtcNow, accountBalance.AccountName);
                }

                return accountBalance;
            }
            catch (Exception)
            {
                throw new Exception("Bank account not found.");
            }
        }

        private string GetMaskedToken(string token)
        {
            string visiblePart = token.Substring(0, 4);
            string maskedPart = new string('*', token.Length - 4);

            return $"{visiblePart}{maskedPart}";
        }
    }
}
