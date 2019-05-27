using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<InventoryItem>> GetItemsAsync(Guid userOpenId);

        Task<InventoryItem> GetItemAsync(Guid userOpenId, Guid id);

        Task<InventoryItem> CreateAsync(Guid userOpenId, string name, string description, string category);

        Task<InventoryItem> UpdateAsync(Guid userOpenId, Guid id, string name, string description, string category);

        Task<bool> RemoveAsync(Guid userOpenId, Guid id);
    }
}
