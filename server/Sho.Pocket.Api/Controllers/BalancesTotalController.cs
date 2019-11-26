using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.Balances;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// GET: api/balances-total/USD
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
