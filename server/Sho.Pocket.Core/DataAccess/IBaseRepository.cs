using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetEntities(string queryText, object queryParameters = null);

        Task<T> GetEntity(string queryText, object queryParameters = null);

        Task<T> InsertEntity(string queryText, object queryParameters = null);

        Task<T> UpdateEntity(string queryText, object queryParameters = null);

        Task DeleteEntity(string queryText, object queryParameters = null);

        Task ExecuteScript(string queryText, object queryParameters = null);

        Task<string> GetQueryText(string dirPath, string fileName);
    }
}
