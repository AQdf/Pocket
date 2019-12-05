using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.BankIntegration.Monobank.Abstractions;
using Sho.Pocket.Core.BankIntegration.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.Accounts.Models;
using Sho.Pocket.Core.Features.BankAccounts.Abstractions;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Features.BankSync
{
    public class AccountBankSyncService : IAccountBankSyncService
    {
        private readonly IMonobankAccountService _monobankAccountService;

        private readonly IBankRepository _bankRepository;

        private readonly IUserBankAuthDataRepository _userBankAuthDataRepository;

        private readonly IAssetBankAccountRepository _assetBankAccountRepository;

        public AccountBankSyncService(
            IMonobankAccountService monobankAccountService,
            IBankRepository bankRepository,
            IUserBankAuthDataRepository userBankAuthDataRepository,
            IAssetBankAccountRepository assetBankAccountRepository)
        {
            _monobankAccountService = monobankAccountService;
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

        public async Task<List<BankAccount>> SubmitBankClientAuthDataAsync(Guid userId, string bankName, string token = null)
        {
            //TODO: Catch errors. Verify if token is correct and then save it to database. Encrypt this token.
            List<BankAccountBalance> accountBalances = await _monobankAccountService.GetClientAccountsInfoAsync(token);
            List<BankAccount> result = new List<BankAccount>();

            if (accountBalances != null)
            {
                UserBankAuthData authData = await _userBankAuthDataRepository.AlterAsync(userId, bankName, token);
                result = accountBalances.Select(a => GetBankAccount(a, bankName, authData.Id)).ToList();
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

        private BankAccount GetBankAccount(BankAccountBalance accountBalance, string bankName, Guid authDataId)
        {
            string accountName = $"{bankName}: {accountBalance.Balance} {accountBalance.Currency}";

            BankAccount result = new BankAccount
            {
                AuthDataId = authDataId,
                Id = accountBalance.AccountId,
                Name = accountName
            };

            return result;
        }

        private string GetMaskedToken(string token)
        {
            string visiblePart = token.Substring(0, 4);
            string maskedPart = new string('*', token.Length - 4);

            return $"{visiblePart}{maskedPart}";
        }
    }
}
