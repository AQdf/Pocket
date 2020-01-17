using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;
using Sho.Pocket.Core.Features.Balances.Abstractions;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/balances/import")]
    public class BalancesImportController : AuthUserControllerBase
    {
        private readonly IBalanceImportService _balanceImportService;

        public BalancesImportController(IBalanceImportService balanceImportService, IAuthService authService) : base(authService)
        {
            _balanceImportService = balanceImportService;
        }

        [HttpPost("json")]
        public async Task<IActionResult> ImportJson()
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            IFormFile file = Request.Form.Files[0];

            if (file.Length > 0)
            {
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    string data = await stream.ReadToEndAsync();
                    await _balanceImportService.ImportJsonAsync(user.Id, data);
                }
            }
            else
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
