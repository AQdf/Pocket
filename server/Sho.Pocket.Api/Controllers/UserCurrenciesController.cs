using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.UserCurrencies;
using Sho.Pocket.Application.UserCurrencies.Models;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/user-currencies")]
    public class UserCurrenciesController : AuthUserControllerBase
    {
        private readonly IUserCurrencyService _userCurrencyService;

        public UserCurrenciesController(IUserCurrencyService userCurrencyService, IAuthService authService) : base(authService)
        {
            _userCurrencyService = userCurrencyService;
        }

        /// <summary>
        /// GET: api/user-currencies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<UserCurrencyModel>>> GetUserCurrencies()
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            List<UserCurrencyModel> result = await _userCurrencyService.GetUserCurrenciesAsync(user.Id);

            return HandleResult(result);
        }

        /// <summary>
        /// GET: api/user-currencies/USD
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        [HttpGet("{currency}")]
        public async Task<ActionResult<UserCurrencyModel>> GetUserCurrency(string currency)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            UserCurrencyModel result = await _userCurrencyService.GetUserCurrencyAsync(user.Id, currency);

            return HandleResult(result);
        }

        /// <summary>
        /// POST: api/user-currencies
        /// </summary>
        /// <param name="createModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<UserCurrencyModel>> AddUserCurrency([FromBody] UserCurrencyCreateModel createModel)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            UserCurrencyModel result = await _userCurrencyService.AddUserCurrencyAsync(user.Id, createModel.Currency, createModel.IsPrimary);

            return HandleResult(result);
        }

        /// <summary>
        /// PATCH: api/user-currencies/EUR
        /// </summary>
        /// <param name="currency"></param>
        [HttpPatch("{currency}")]
        public async Task<ActionResult<UserCurrencyModel>> SetUserPrimaryCurrency(string currency)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            UserCurrencyModel result = await _userCurrencyService.SetUserPrimaryCurrencyAsync(user.Id, currency);

            return HandleResult(result);
        }

        /// <summary>
        /// DELETE: api/user-currencies/USD
        /// </summary>
        /// <param name="currency"></param>
        [HttpDelete("{currency}")]
        public async Task<ActionResult<bool>> DeleteUserCurrency(string currency)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            bool result = await _userCurrencyService.DeleteUserCurrencyAsync(user.Id, currency);

            return Ok(result);
        }
    }
}
