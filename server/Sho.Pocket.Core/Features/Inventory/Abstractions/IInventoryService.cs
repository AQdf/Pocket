using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.Inventory.Models;

namespace Sho.Pocket.Core.Features.Inventory.Abstractions
{
    public interface IInventoryService
    {
        Task<List<InventoryItemModel>> GetItemsAsync(Guid userOpenId);

        Task<InventoryItemModel> GetItemAsync(Guid userOpenId, Guid id);

        Task<InventoryItemModel> AddItemAsync(Guid userOpenId, InventoryItemCreateModel createModel);

        Task<InventoryItemModel> UpdateItemAsync(Guid userOpenId, Guid id, InventoryItemUpdateModel updateModel);

        Task<bool> DeleteItemAsync(Guid userOpenId, Guid id);
    }
}
