using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;
using System.Threading.Tasks;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILoginService _loginService;

        private readonly IRegistrationService _registrationService;

        public UsersController(ILoginService loginService, IRegistrationService registrationService)
        {
            _loginService = loginService;
            _registrationService = registrationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _registrationService.CreateUser(model);

            return result.Succeeded ? Ok(result) : (IActionResult)BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            IActionResult response;

            if (ModelState.IsValid)
            {
                var identity = await _loginService.GetClaimsIdentity(model.Email, model.Password);

                if (identity != null)
                {
                    string jwt = await _loginService.GenerateJwt(model.Email, identity);

                    response = Ok(jwt);

                }
                else
                {
                    response = Unauthorized();
                }
            }
            else
            {
                response = BadRequest(ModelState);
            }

            return response;
        }
    }
}