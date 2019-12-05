using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IBankRepository
    {
        Task<IEnumerable<Bank>> GetSupportedBanksAsync();
    }
}
