using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;
using Sho.Pocket.Core.Features.Balances.Abstractions;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/balances/export")]
    public class BalancesExportController : AuthUserControllerBase
    {
        private readonly IBalanceExportService _balanceExportService;

        public BalancesExportController(IBalanceExportService balanceExportService, IAuthService authService) : base(authService)
        {
            _balanceExportService = balanceExportService;
        }

        [HttpGet("csv")]
        public async Task<IActionResult> DownloadAllCsv()
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            byte[] bytes = await _balanceExportService.ExportUserBalancesToCsvAsync(user.Id);

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

            byte[] bytes = await _balanceExportService.ExportJsonAsync(user.Id, effectiveDate);

            return File(bytes, "application/json");
        }
    }
}
