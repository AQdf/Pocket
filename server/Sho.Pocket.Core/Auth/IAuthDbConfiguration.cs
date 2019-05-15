using System.Threading.Tasks;

namespace Sho.Pocket.Core.Auth
{
    public interface IAuthDbConfiguration
    {
        Task SeedApplicationAuthData();
    }
}
