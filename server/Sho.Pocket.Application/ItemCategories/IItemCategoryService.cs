using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Application.ItemCategories
{
    public interface IItemCategoryService
    {
        Task<IEnumerable<string>> GetDefaultCategoriesAsync();

        Task<bool> ExistsAsync(string name);

        Task<string> AddDefaultCategoryAsync(string name);

        Task<string> UpdateDefaultCategoryAsync(string oldName, string newName);

        Task<bool> DeleteDefaultCategoryAsync(string name);
    }
}
