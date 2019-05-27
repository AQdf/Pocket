using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Auth.IdentityServer.Services;
using Sho.Pocket.Auth.IdentityServer.Utils;
using System.Threading.Tasks;

namespace Sho.Pocket.Api.AdminControllers
{
    [Authorize(Policy = AuthPolicyConst.AdminUser)]
    [ApiController]
    public abstract class AuthAdminControllerBase : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthAdminControllerBase(IAuthService authService)
        {
            _authService = authService;
        }

        protected async Task<bool> VerifyCurrentAdminUserAsync()
        {
            bool result = false;
            string id = User.FindFirst("id")?.Value;

            if (!string.IsNullOrWhiteSpace(id))
            {
                result = await _authService.VerifyAdminUserById(id);
            }

            return result;
        }

        protected ActionResult HandleUserNotAdminResult()
        {
            object error = "User is not admin";
            return BadRequest(error);
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
