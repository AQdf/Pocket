using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.ExchangeRates.Abstractions;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;
using System;
using System.Collections.Generic;
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

        [HttpGet("{effectiveDate}")]
        public async Task<List<ExchangeRateModel>> GetExchangeRates(DateTime effectiveDate)
        {
            List<ExchangeRateModel> result = await _exchangeRateService.GetExchangeRatesAsync(effectiveDate);

            return result;
        }

        [HttpGet("providers/{provider}")]
        public async Task<ActionResult<List<ExchangeRateProviderModel>>> GetProviderExchangeRate(string provider)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            List<ExchangeRateProviderModel> result = await _exchangeRateService.GetProviderExchangeRateAsync(user.Id, provider);

            return result;
        }

        [HttpPut]
        public async Task<bool> UpdateExchangeRate([FromBody]ExchangeRateModel model)
        {
            await _exchangeRateService.UpdateExchangeRateAsync(model);

            return true;
        }
    }
}
