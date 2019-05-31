using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Application.BalancesTotal;
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

            List<BalanceTotalModel> result = await _balancesTotalService.GetCurrentTotalBalance(user.Id);

            return HandleResult(result);
        }

        /// <summary>
        /// GET: api/balances-total/USD
        /// </summary>
        /// <returns></returns>
        [HttpGet("{currencyName}")]
        public async Task<ActionResult<List<BalanceTotalModel>>> GetCurrencyTotals(string currencyName, [FromQuery] int count = 10)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            IEnumerable<BalanceTotalModel> result = await _balancesTotalService.GetCurrencyTotals(user.Id, currencyName, count);

            return HandleResult(result);
        }
    }
}
