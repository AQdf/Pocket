using System.Threading.Tasks;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
