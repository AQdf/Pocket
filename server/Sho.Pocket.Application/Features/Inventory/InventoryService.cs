using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.Inventory.Abstractions;
using Sho.Pocket.Core.Features.Inventory.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<List<InventoryItemModel>> GetItemsAsync(Guid userOpenId)
        {
            IEnumerable<InventoryItem> items = await _inventoryRepository.GetItemsAsync(userOpenId);
            List<InventoryItemModel> result = items?.Select(i => new InventoryItemModel(i)).ToList();

            return result;
        }

        public async Task<InventoryItemModel> GetItemAsync(Guid userOpenId, Guid id)
        {
            InventoryItem item = await _inventoryRepository.GetItemAsync(userOpenId, id);
            InventoryItemModel result = new InventoryItemModel(item);

            return result;
        }

        public async Task<InventoryItemModel> AddItemAsync(Guid userOpenId, InventoryItemCreateModel createModel)
        {
            InventoryItem item = await _inventoryRepository.CreateAsync(userOpenId, createModel.Name, createModel.Description, createModel.Category);
            InventoryItemModel result = new InventoryItemModel(item);

            return result;
        }

        public async Task<InventoryItemModel> UpdateItemAsync(Guid userOpenId, Guid id, InventoryItemUpdateModel updateModel)
        {
            InventoryItem item = await _inventoryRepository.UpdateAsync(userOpenId, id, updateModel.Name, updateModel.Description, updateModel.Category);
            InventoryItemModel result = new InventoryItemModel(item);

            return result;
        }

        public async Task<bool> DeleteItemAsync(Guid userOpenId, Guid id)
        {
            bool result = await _inventoryRepository.RemoveAsync(userOpenId, id);

            return result;
        }
    }
}
