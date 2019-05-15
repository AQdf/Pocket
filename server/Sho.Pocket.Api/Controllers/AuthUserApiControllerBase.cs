using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;
using System.Threading.Tasks;

namespace Sho.Pocket.Api.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [ApiController]
    public abstract class AuthUserApiControllerBase : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthUserApiControllerBase(IAuthService authService)
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
            object error = "User not found";
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
