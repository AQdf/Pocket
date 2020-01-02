using System.Threading.Tasks;

namespace Sho.Pocket.Auth.IdentityServer.Configuration
{
    public interface IAuthDbInitializer
    {
        Task SeedApplicationAuthData();
    }
}
