using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.Balances;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/balances")]
    public class BalancesController : AuthUserApiControllerBase
    {
        private readonly IBalanceService _balanceService;

        public BalancesController(IBalanceService balanceService, IAuthService authService) : base(authService)
        {
            _balanceService = balanceService;
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
        /// GET: api/balances/total
        /// </summary>
        /// <returns></returns>
        [HttpGet("total")]
        public async Task<ActionResult<List<BalanceTotalModel>>> GetCurrentTotalBalance()
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            List<BalanceTotalModel> result = await _balanceService.GetCurrentTotalBalance(user.Id);

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

        [HttpPut("exchange-rate")]
        public async Task<bool> ApplyExchangeRate([FromBody]ExchangeRateModel model)
        {
            await _balanceService.ApplyExchangeRate(model);

            return true;
        }

        [HttpGet("currency-totals/{currencyName}")]
        public async Task<ActionResult<List<BalanceTotalModel>>> GetCurrencyTotals(string currencyName, [FromQuery] int count = 10)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            IEnumerable<BalanceTotalModel> result = await _balanceService.GetCurrencyTotals(user.Id, currencyName, count);

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