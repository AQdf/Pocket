using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IAssetBankAccountRepository
    {
        Task<IList<AssetBankAccount>> GetByUserIdAsync(Guid userId);

        Task<AssetBankAccount> CreateAsync(Guid userId, Guid assetId, string bankName, string accountName, string bankAccountId);

        Task<AssetBankAccount> GetAsync(Guid userId, Guid assetId);

        Task DeleteAsync(Guid userId, Guid assetId);

        Task<AssetBankAccount> UpdateAsync(Guid userId, Guid id, DateTime lastSyncDateTime, string bankAccountName);
    }
}
