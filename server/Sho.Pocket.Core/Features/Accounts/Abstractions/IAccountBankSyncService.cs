using Sho.Pocket.Core.Features.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.Features.BankAccounts.Abstractions
{
    public interface IAccountBankSyncService
    {
        Task<AssetBankSyncData> GetAssetBankAccountAsync(Guid userId, Guid assetId);

        Task<List<string>> GetSupportedBanksAsync();

        Task<List<BankAccount>> SubmitBankClientAuthDataAsync(Guid userId, string bankName, string token = null);

        Task<bool> ConnectAssetWithBankAcountAsync(Guid userId, Guid assetId, string bankName, string accountName, string bankAccountId);

        Task<bool> DisconnectAssetWithBankAcountAsync(Guid userId, Guid assetId);
    }
}
