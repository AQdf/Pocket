using System.Security.Claims;
using System.Threading.Tasks;

namespace Sho.Pocket.Auth.IdentityServer.Services
{
    public interface ILoginService
    {
        Task<ClaimsIdentity> GetClaimsIdentity(string email, string password);

        Task<string> GenerateJwt(string userName, ClaimsIdentity identity);
    }
}
