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

            return result.Succeeded 
                ? Ok(result) 
                : (IActionResult)BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            LoginResult result = await _loginService.GenerateJwtAsync(model.Email, model.Password);

            return result.Succeeded
                ? Ok(result.Jwt) 
                : (IActionResult)Unauthorized();
        }
    }
}