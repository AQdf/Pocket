using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Core.Auth;
using System.Collections.Generic;
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

        public async Task<LoginResult> GenerateJwtAsync(string email, string password)
        {
            LoginResult result;
            ApplicationUser user = await GetValidatedUserAsync(email, password);

            if (user != null)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);
                string authToken = await _jwtGenerator.GenerateEncodedTokenAsync(user.Id, user.Email, roles);

                var response = new
                {
                    id = user.Id,
                    auth_token = authToken,
                    expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
                };

                string jsonResponse = JsonConvert.SerializeObject(response, _serializerSettings);
                result = LoginResult.Success(jsonResponse);
            }
            else
            {
                result = LoginResult.Failed();
            }

            return result;
        }

        private async Task<ApplicationUser> GetValidatedUserAsync(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(userName);

                if (user != null)
                {
                    bool isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

                    if (isPasswordValid)
                    {
                        return user;
                    }
                }
            }

            return null;
        }
    }
}
