using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sho.Pocket.Auth.IdentityServer.Services
{
    public interface IJwtGenerator
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);

        ClaimsIdentity GenerateClaimsIdentity(string userName, Guid id);
    }
}
