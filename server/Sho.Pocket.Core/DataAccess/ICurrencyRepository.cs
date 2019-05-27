using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.DataAccess
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<Currency>> GetAllAsync();

        Task<bool> ExistsAsync(string name);

        Task<Currency> CreateAsync(string name);
    }
}
