using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.Core.BankIntegration;
using Sho.Pocket.Core.BankIntegration.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.BankAccounts.Abstractions;
using Sho.Pocket.Core.Features.BankAccounts.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Features.BankSync
{
    public class AccountBankSyncService : IBankAccountSyncService
    {
        private readonly IBankAccountServiceResolver _bankAccountServiceResolver;

        private readonly IBankRepository _bankRepository;

        private readonly IAssetBankAccountRepository _assetBankAccountRepository;

        public AccountBankSyncService(
            IBankAccountServiceResolver bankAccountServiceResolver,
            IBankRepository bankRepository,
            IAssetBankAccountRepository assetBankAccountRepository)
        {
            _bankAccountServiceResolver = bankAccountServiceResolver;
            _bankRepository = bankRepository;
            _assetBankAccountRepository = assetBankAccountRepository;
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
            List<BankAccountBalance> accountBalances = await bankAccountservice.GetClientAccountsInfoAsync(clientData);
            List<BankAccount> result = new List<BankAccount>();

            if (accountBalances != null)
            {
                AssetBankAccount bankAccount = await _assetBankAccountRepository.AlterAsync(userId, assetId, bankName, token, bankClientId);
                result = accountBalances.Select(a => new BankAccount(a.AccountId, a.AccountName)).ToList();
            }
            else
            {
                throw new Exception($"Sync with bank {bankName} failed.");
            }

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
            try
            {
                BankAccountBalance accountBalance = null;
                AssetBankAccount assetBankAccount = await _assetBankAccountRepository.GetAsync(userId, assetId);
                Bank bank = await _bankRepository.GetBankAsync(assetBankAccount.BankName);
                TimeSpan syncSpan = DateTime.UtcNow - assetBankAccount.LastSyncDateTime;

                if (syncSpan.TotalSeconds > bank.SyncFreqInSeconds)
                {
                    IBankAccountService bankAccountservice = _bankAccountServiceResolver.Resolve(assetBankAccount.BankName);
                    BankClientData clientData = new BankClientData(assetBankAccount.Token, assetBankAccount.BankClientId, assetBankAccount.BankAccountId);
                    List<BankAccountBalance> accountBalances = await bankAccountservice.GetClientAccountsInfoAsync(clientData);
                    accountBalance = accountBalances.FirstOrDefault(ab => ab.AccountId == assetBankAccount.BankAccountId);

                    await _assetBankAccountRepository.UpdateLatsSyncAsync(userId, assetId, DateTime.UtcNow, accountBalance.AccountName);
                }

                return accountBalance;
            }
            catch (Exception)
            {
                throw new Exception("Bank account not found.");
            }
        }
    }
}
