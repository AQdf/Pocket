using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.Assets;
using Sho.Pocket.Application.Assets.Models;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public DashboardController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        /// <summary>
        /// GET: api/dashboard/total
        /// </summary>
        /// <returns></returns>
        [HttpGet("total")]
        public decimal GetTotalBalance()
        {
            IEnumerable<AssetViewModel> assets = _assetService.GetAll();

            IEnumerable<string> currencies = assets.Select(a => a.CurrencyName).Distinct();

            decimal result = assets.Select(asset => decimal.Multiply(asset.Balance, 1.0M)).Sum();

            return result;
        }
    }
}