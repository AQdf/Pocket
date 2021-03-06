﻿using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IBankAccountRepository
    {
        Task<IList<AssetBankAccount>> GetByUserIdAsync(Guid userId);

        Task<AssetBankAccount> GetByAssetIdAsync(Guid userId, Guid assetId);

        Task<AssetBankAccount> AlterAsync(Guid userId, Guid assetId, string bankName, string token, string bankClientId);

        Task<AssetBankAccount> UpdateAccountAsync(Guid userId, Guid assetId, string accountName, string bankAccountId);

        Task DeleteAsync(Guid userId, Guid assetId);

        Task<AssetBankAccount> UpdateLastSyncAsync(Guid userId, Guid id, DateTime lastSyncDateTime, string bankAccountName);
    }
}
