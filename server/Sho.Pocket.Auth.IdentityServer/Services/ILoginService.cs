using Sho.Pocket.Auth.IdentityServer.Models;
using System.Threading.Tasks;

namespace Sho.Pocket.Auth.IdentityServer.Services
{
    public interface ILoginService
    {
        Task<LoginResult> GenerateJwtAsync(string email, string password);
    }
}
