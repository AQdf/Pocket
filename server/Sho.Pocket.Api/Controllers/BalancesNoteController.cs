using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.Features.Balances;
using Sho.Pocket.Application.Features.Balances.Models;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;
using System;
using System.Threading.Tasks;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/balances/notes")]
    public class BalancesNoteController : AuthUserControllerBase
    {
        private readonly IBalanceNoteService _balanceNoteService;

        public BalancesNoteController(IBalanceNoteService balanceNoteService, IAuthService authService) : base(authService)
        {
            _balanceNoteService = balanceNoteService;
        }

        /// <summary>
        /// GET: api/balances/notes/2019-09-10 00:00:00.0000000
        /// </summary>
        /// <param name="effectiveDate"></param>
        /// <returns></returns>
        [HttpGet("{effectiveDate}")]
        public async Task<ActionResult<BalanceNoteViewModel>> GetNoteByEffectiveDateAsync(DateTime effectiveDate)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            BalanceNoteViewModel result = await _balanceNoteService.GetNoteByEffectiveDateAsync(user.Id, effectiveDate);

            return Ok(result);
        }

        /// <summary>
        /// PATCH: api/balances/notes/2019-09-10 00:00:00.0000000
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        [HttpPatch("{effectiveDate}")]
        public async Task<ActionResult<BalanceNoteViewModel>> AlterNoteAsync(DateTime effectiveDate, [FromBody] BalanceNoteUpdateModel note)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            BalanceNoteViewModel result = await _balanceNoteService.AlterNoteAsync(user.Id, effectiveDate, note.Content);

            return Ok(result);
        }
    }
}
