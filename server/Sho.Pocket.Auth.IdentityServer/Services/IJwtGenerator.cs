using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Auth.IdentityServer.Services
{
    public interface IJwtGenerator
    {
        Task<string> GenerateEncodedTokenAsync(Guid userId, string email, IList<string> roles);
    }
}
