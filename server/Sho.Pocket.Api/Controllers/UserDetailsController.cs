using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Api.Models;
using Sho.Pocket.Application.UserCurrencies;
using Sho.Pocket.Application.UserCurrencies.Models;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/user/details")]
    public class UserDetailsController : AuthUserControllerBase
    {
        private readonly IUserCurrencyService _userCurrencyService;

        public UserDetailsController(IUserCurrencyService userCurrencyService, IAuthService authService) : base(authService)
        {
            _userCurrencyService = userCurrencyService;
        }

        [HttpGet]
        public async Task<ActionResult<UserDetailsModel>> GetUserDetails()
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            List<UserCurrencyModel> userCurrencies = await _userCurrencyService.GetUserCurrenciesAsync(user.Id);
            UserDetailsModel result = new UserDetailsModel(user.Email, userCurrencies);

            return Ok(result);
        }
    }
}
