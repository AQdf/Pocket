using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;
using Sho.Pocket.Auth.IdentityServer.Utils;
using System.Threading.Tasks;

namespace Sho.Pocket.Api.Controllers
{
    [Authorize(Policy = AuthPolicyConst.SimpleUser)]
    [ApiController]
    public abstract class AuthUserControllerBase : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthUserControllerBase(IAuthService authService)
        {
            _authService = authService;
        }

        protected async Task<UserViewModel> GetCurrentUserAsync()
        {
            UserViewModel result = null;

            string id = User.FindFirst("id")?.Value;

            if (!string.IsNullOrWhiteSpace(id))
            {
                result = await _authService.GetUserById(id);
            }

            return result;
        }

        protected ActionResult HandleUserNotFoundResult()
        {
            object error = "User not found!";
            return Unauthorized(error);
        }

        protected ActionResult HandleResult(object result)
        {
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
