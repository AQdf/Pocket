using System.Threading.Tasks;

namespace Sho.Pocket.Core.Auth
{
    public interface IAuthDbInitializer
    {
        Task SeedApplicationAuthData();
    }
}
