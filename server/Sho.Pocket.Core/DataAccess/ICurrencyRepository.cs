using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.DataAccess
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<string>> GetAllAsync();

        Task<bool> ExistsAsync(string name);

        Task<string> CreateAsync(string name);
    }
}
