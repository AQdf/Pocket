using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        List<T> GetAll(string queryText, object queryParameters = null);

        T GetEntity(string queryText, object queryParameters = null);

        T InsertEntity(string queryText, object queryParameters = null);

        T UpdateEntity(string queryText, object queryParameters = null);

        void RemoveEntity(string queryText, object queryParameters = null);

        void ExecuteScript(string queryText, object queryParameters = null);

        string GetQueryText(string dirPath, string fileName);
    }
}
