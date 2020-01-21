using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;
using Sho.Pocket.Core.Features.Balances.Abstractions;
using Sho.Pocket.Core.Features.Balances.Models;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/balances-total")]
    public class BalancesTotalController : AuthUserControllerBase
    {
        private readonly IBalancesTotalService _balancesTotalService;

        public BalancesTotalController(IBalancesTotalService balancesTotalService, IAuthService authService) : base(authService)
        {
            _balancesTotalService = balancesTotalService;
        }

        /// <summary>
        /// GET: api/balances-total
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<BalanceTotalModel>>> GetCurrentTotalBalance()
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            List<BalanceTotalModel> result = await _balancesTotalService.GetLatestTotalBalanceAsync(user.Id);

            return Ok(result);
        }

        /// <summary>
        /// GET: api/balances-total/primary-currency-balances
        /// </summary>
        /// <returns></returns>
        [HttpGet("primary-currency-balances")]
        public async Task<ActionResult<List<BalancePrimaryCurrencyModel>>> GetUserPrimaryCurrencyBalances()
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            List<BalancePrimaryCurrencyModel> result = await _balancesTotalService.GetUserPrimaryCurrencyBalancesAsync(user.Id);

            return Ok(result);
        }

        /// <summary>
        /// GET: api/balances-total/changes
        /// </summary>
        /// <returns></returns>
        [HttpGet("changes")]
        public async Task<ActionResult<List<BalanceTotalChangeModel>>> GetUserBalanceChanges([FromQuery] int count = 50)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            List<BalanceTotalChangeModel> result = await _balancesTotalService.GetUserBalanceChangesAsync(user.Id, count);

            return HandleResult(result);
        }
    }
}
