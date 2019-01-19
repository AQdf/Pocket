using System;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.Balances;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly BalanceService _balanceService;

        public DashboardController(BalanceService balancesService)
        {
            _balanceService = balancesService;
        }
    }
}