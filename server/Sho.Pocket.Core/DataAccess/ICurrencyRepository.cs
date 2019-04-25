using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.DataAccess
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<Currency>> GetAll();

        Task<Currency> GetByName(string name);

        Task<Currency> Add(string name);
    }
}
