using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Core.Auth;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sho.Pocket.Auth.IdentityServer.Services
{
    public class LoginService : ILoginService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly JwtIssuerOptions _jwtOptions;

        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };

        public LoginService(UserManager<ApplicationUser> userManager, IJwtGenerator jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _jwtGenerator = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            ClaimsIdentity result = null;

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                ApplicationUser userToVerify = await _userManager.FindByEmailAsync(userName);

                if (userToVerify != null)
                {
                    bool isPasswordValid = await _userManager.CheckPasswordAsync(userToVerify, password);

                    if (isPasswordValid)
                    {
                        result = _jwtGenerator.GenerateClaimsIdentity(userName, userToVerify.Id);
                    }
                }
            }

            return result;
        }

        public async Task<string> GenerateJwt(string userName, ClaimsIdentity identity)
        {
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                auth_token = await _jwtGenerator.GenerateEncodedToken(userName, identity),
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
            };

            return JsonConvert.SerializeObject(response, _serializerSettings);
        }
    }
}
