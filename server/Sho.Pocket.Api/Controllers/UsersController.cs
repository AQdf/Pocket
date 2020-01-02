using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Api.Models;
using Sho.Pocket.Application.UserCurrencies;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILoginService _loginService;

        private readonly IRegistrationService _registrationService;

        private readonly IUserCurrencyService _userCurrencyService;

        public UsersController(
            ILoginService loginService,
            IRegistrationService registrationService,
            IUserCurrencyService userCurrencyService)
        {
            _loginService = loginService;
            _registrationService = registrationService;
            _userCurrencyService = userCurrencyService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserCreationResult result = await _registrationService.CreateSimpleUser(model);

            if (result.Succeeded)
            {
                await _userCurrencyService.AddUserCurrencyAsync(result.User.Id, model.PrimaryCurrency, true);

                return Ok(result);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            LoginResult result = await _loginService.GenerateJwtAsync(model.Email, model.Password);

            if (result.Succeeded)
            {
                return Ok(result.Jwt);
            }
            else
            {
                List<ResponseError> errors = new List<ResponseError>
                {
                    new ResponseError("InvalidEmailOrPassword", "Invalid email or password")
                };

                return BadRequest(errors);
            }
        }
    }
}