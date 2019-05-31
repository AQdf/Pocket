using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Auth.IdentityServer.Services;
using System.Threading.Tasks;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/exchange-rates")]
    public class ExchangeRatesController : AuthUserControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRatesController(IExchangeRateService exchangeRateService, IAuthService authService) : base(authService)
        {
            _exchangeRateService = exchangeRateService;
        }

        [HttpPut]
        public async Task<bool> UpdateExchangeRate([FromBody]ExchangeRateModel model)
        {
            await _exchangeRateService.UpdateExchangeRateAsync(model);

            return true;
        }
    }
}
