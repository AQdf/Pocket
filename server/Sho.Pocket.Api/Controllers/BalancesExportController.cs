using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.Balances;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;
using System;
using System.Threading.Tasks;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/balances/export")]
    public class BalancesExportController : AuthUserControllerBase
    {
        private readonly IBalanceService _balanceService;

        public BalancesExportController(IBalanceService balanceService, IAuthService authService) : base(authService)
        {
            _balanceService = balanceService;
        }

        [HttpGet("csv")]
        public async Task<IActionResult> DownloadAllCsv()
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            byte[] bytes = await _balanceService.ExportUserBalancesToCsvAsync(user.Id);

            return File(bytes, "application/csv");
        }

        [HttpGet("json/{effectiveDate}")]
        public async Task<IActionResult> ExportJson(DateTime effectiveDate)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            byte[] bytes = await _balanceService.ExportJsonAsync(user.Id, effectiveDate);

            return File(bytes, "application/json");
        }
    }
}
