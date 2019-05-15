using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.Balances;
using Sho.Pocket.Auth.IdentityServer.Services;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/dashboard")]
    public class DashboardController : AuthUserApiControllerBase
    {
        private readonly BalanceService _balanceService;

        public DashboardController(BalanceService balancesService, IAuthService authService) : base(authService)
        {
            _balanceService = balancesService;
        }
    }
}