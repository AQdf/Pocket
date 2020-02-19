using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;

namespace Sho.Pocket.DataAccess.Sql.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PocketDbContext _context;

        public UnitOfWork(PocketDbContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
