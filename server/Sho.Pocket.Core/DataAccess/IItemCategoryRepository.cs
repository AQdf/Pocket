using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IItemCategoryRepository
    {
        Task<IEnumerable<ItemCategory>> GetAllAsync();

        Task<bool> ExistsAsync(string name);

        Task<ItemCategory> CreateAsync(string name);

        Task<ItemCategory> UpdateAsync(string oldName, string newName);

        Task<bool> DeleteAsync(string name);
    }
}
