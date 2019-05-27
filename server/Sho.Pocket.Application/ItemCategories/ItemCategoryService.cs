using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sho.Pocket.Application.ItemCategories
{
    public class ItemCategoryService : IItemCategoryService
    {
        private readonly IItemCategoryRepository _itemCategoryRepository;

        public ItemCategoryService(IItemCategoryRepository itemCategoryRepository)
        {
            _itemCategoryRepository = itemCategoryRepository;
        }

        public async Task<IEnumerable<string>> GetDefaultCategoriesAsync()
        {
            IEnumerable<ItemCategory> categories = await _itemCategoryRepository.GetAllAsync();
            List<string> result = categories.Select(c => c.Name).ToList();

            return result;
        }

        public async Task<bool> ExistsAsync(string name)
        {
            bool result = await _itemCategoryRepository.ExistsAsync(name);

            return result;
        }

        public async Task<string> AddDefaultCategoryAsync(string name)
        {
            ItemCategory category = await _itemCategoryRepository.CreateAsync(name);

            return category.Name;
        }

        public async Task<string> UpdateDefaultCategoryAsync(string oldName, string newName)
        {
            ItemCategory category = await _itemCategoryRepository.UpdateAsync(oldName, newName);

            return category.Name;
        }

        public async Task<bool> DeleteDefaultCategoryAsync(string name)
        {
            bool result = await _itemCategoryRepository.DeleteAsync(name);

            return result;
        }
    }
}
