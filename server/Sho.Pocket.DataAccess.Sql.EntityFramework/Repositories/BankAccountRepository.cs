using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.EntityFramework.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly DbSet<AssetBankAccount> _set;

        public BankAccountRepository(PocketDbContext context)
        {
            _set = context.Set<AssetBankAccount>();
        }

        public async Task<AssetBankAccount> GetAsync(Guid userId, Guid assetId)
        {
            return await _set.FirstAsync(ba => ba.AssetId == assetId && ba.Asset.UserOpenId == userId);
        }

        public async Task<AssetBankAccount> AlterAsync(Guid userId, Guid assetId, string bankName, string token, string bankClientId)
        {
            AssetBankAccount bankAccount = await _set.FirstOrDefaultAsync(ba => ba.AssetId == assetId && ba.Asset.UserOpenId == userId);
            EntityEntry<AssetBankAccount> result;

            if (bankAccount != null)
            {
                bankAccount.BankName = bankName;
                bankAccount.Token = token;
                bankAccount.BankClientId = bankClientId;
                result = _set.Update(bankAccount);
            }
            else
            {
                AssetBankAccount newAccount = new AssetBankAccount(Guid.NewGuid(), assetId, bankName, token, bankClientId);
                result = await _set.AddAsync(newAccount);
            }

            return result.Entity;
        }

        public async Task DeleteAsync(Guid userId, Guid assetId)
        {
            AssetBankAccount bankAccount = await _set.FirstAsync(ba => ba.AssetId == assetId && ba.Asset.UserOpenId == userId);
            _set.Remove(bankAccount);
        }

        public async Task<AssetBankAccount> UpdateAccountAsync(Guid userId, Guid assetId, string accountName, string accountId)
        {
            AssetBankAccount bankAccount = await _set.FirstAsync(ba => ba.AssetId == assetId && ba.Asset.UserOpenId == userId);
            bankAccount.BankAccountName = accountName;
            bankAccount.BankAccountId = accountId;
            EntityEntry<AssetBankAccount> result = _set.Update(bankAccount);

            return result.Entity;
        }

        public async Task<AssetBankAccount> UpdateLastSyncAsync(Guid userId, Guid assetId, DateTime lastSyncDateTime, string accountName)
        {
            AssetBankAccount bankAccount = await _set.FirstAsync(ba => ba.AssetId == assetId && ba.Asset.UserOpenId == userId);
            bankAccount.LastSyncDateTime = lastSyncDateTime;
            bankAccount.BankAccountName = accountName;
            EntityEntry<AssetBankAccount> result = _set.Update(bankAccount);

            return result.Entity;
        }

        public async Task<bool> ExistsAsync(Guid userId, Guid assetId)
        {
            return await _set.AnyAsync(
                ba => ba.AssetId == assetId 
                && ba.Asset.UserOpenId == userId 
                && !string.IsNullOrWhiteSpace(ba.BankAccountId));

        }

        public async Task<IList<AssetBankAccount>> GetByUserIdAsync(Guid userId)
        {
            return await _set.Where(ba => ba.Asset.UserOpenId == userId).ToListAsync();
        }
    }
}
