using Sho.Pocket.Core.BankIntegration.Models;
using Sho.Pocket.Core.Features.BankAccounts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.Features.BankAccounts.Abstractions
{
    public interface IBankAccountSyncService
    {
        Task<AssetBankAccountViewModel> GetAssetBankAccountAsync(Guid userId, Guid assetId);

        Task<List<string>> GetSupportedBanksAsync();

        Task<List<BankAccount>> SubmitBankClientAuthDataAsync(Guid userId, Guid assetId, string bankName, string token, string bankClientId, string cardNumber);

        Task<bool> ConnectAssetWithBankAcountAsync(Guid userId, Guid assetId, string bankName, string accountName, string bankAccountId);

        Task<bool> DisconnectAssetWithBankAcountAsync(Guid userId, Guid assetId);

        Task<BankAccountBalance> GetBankAccountBalanceAsync(Guid userId, Guid assetId);
    }
}
