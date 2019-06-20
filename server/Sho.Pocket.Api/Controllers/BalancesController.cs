using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.Balances;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/balances")]
    public class BalancesController : AuthUserControllerBase
    {
        private readonly IBalanceService _balanceService;

        public BalancesController(IBalanceService balanceService, IAuthService authService) : base(authService)
        {
            _balanceService = balanceService;
        }

        /// <summary>
        /// GET: api/balances/latest
        /// </summary>
        /// <returns></returns>
        [HttpGet("latest")]
        public async Task<ActionResult<BalancesViewModel>> GetCurrentUserLatestBalances()
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            BalancesViewModel result = await _balanceService.GetUserLatestBalancesAsync(user.Id);

            return HandleResult(result);
        }

        /// <summary>
        /// GET: api/balances
        /// </summary>
        /// <returns></returns>
        [HttpGet("{effectiveDate}")]
        public async Task<ActionResult<BalancesViewModel>> GetCurrentUserEffectiveBalances(DateTime effectiveDate)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            BalancesViewModel result = await _balanceService.GetUserEffectiveBalancesAsync(user.Id, effectiveDate);

            return HandleResult(result);
        }

        /// <summary>
        /// GET: api/balances/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BalanceViewModel>> GetCurrentUserBalance(Guid id)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            BalanceViewModel result = await _balanceService.GetUserBalanceAsync(user.Id, id);

            return HandleResult(result);
        }

        /// <summary>
        /// POST: api/balances
        /// </summary>
        /// <param name="balanceModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<BalanceViewModel>> AddBalance([FromBody] BalanceCreateModel createModel)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            BalanceViewModel result = await _balanceService.AddBalanceAsync(user.Id, createModel);

            return HandleResult(result);
        }

        /// <summary>
        /// PUT: api/balances/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="balanceModel"></param>
        [HttpPut("{id}")]
        public async Task<ActionResult<BalanceViewModel>> UpdateBalance(Guid id, [FromBody] BalanceUpdateModel updateModel)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            BalanceViewModel result = await _balanceService.UpdateBalanceAsync(user.Id, id, updateModel);

            return HandleResult(result);
        }

        /// <summary>
        /// DELETE: api/balances/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="Id"></param>
        [HttpDelete("{Id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            var result = await _balanceService.DeleteBalanceAsync(user.Id, id);

            return HandleResult(result);
        }

        /// <summary>
        /// POST: api/balances/template
        /// </summary>
        /// <param name="balanceModel"></param>
        /// <returns></returns>
        [HttpPost("template")]
        public async Task<ActionResult<List<BalanceViewModel>>> AddBalancesTemplate()
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            List<BalanceViewModel> result = await _balanceService.AddEffectiveBalancesTemplate(user.Id);

            return HandleResult(result);
        }

        /// <summary>
        /// GET: api/balances/effective-dates
        /// </summary>
        /// <returns></returns>
        [HttpGet("effective-dates")]
        public async Task<ActionResult<List<DateTime>>> GetCurrentUserBalancesDates()
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            List<DateTime> result = await _balanceService.GetEffectiveDatesAsync(user.Id);

            return HandleResult(result);
        }

        [HttpGet("csv")]
        public async Task<IActionResult> DownloadCsv()
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            byte[] bytes = await _balanceService.ExportUserBalancesToCsvAsync(user.Id);

            return File(bytes, "application/csv");
        }
    }
}