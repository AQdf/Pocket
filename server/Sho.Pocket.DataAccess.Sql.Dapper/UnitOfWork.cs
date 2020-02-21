using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;

namespace Sho.Pocket.DataAccess.Sql.Dapper
{
    public class UnitOfWork : IUnitOfWork
    {
        public Task SaveChangesAsync()
        {
            // Query is executed in repository when using Dapper Data Access implementation,
            // so nothing to save here;
            return Task.CompletedTask;
        }
    }
}
