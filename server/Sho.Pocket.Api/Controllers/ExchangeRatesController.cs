using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Auth.IdentityServer.Models;
using Sho.Pocket.Auth.IdentityServer.Services;
using Sho.Pocket.Core.Features.ExchangeRates;
using Sho.Pocket.Core.Features.ExchangeRates.Models;

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
        public async Task<ActionResult<List<ExchangeRateModel>>> GetExchangeRates(DateTime effectiveDate)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            List<ExchangeRateModel> result = await _exchangeRateService.GetExchangeRatesAsync(user.Id, effectiveDate);

            return result;
        }

        [HttpGet("providers/{provider}")]
        public async Task<ActionResult<IReadOnlyCollection<ExchangeRateProviderModel>>> GetProviderExchangeRates(string provider)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            IReadOnlyCollection<ExchangeRateProviderModel> result = await _exchangeRateService.FetchProviderExchangeRatesAsync(user.Id, provider);

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateExchangeRate([FromBody]ExchangeRateModel model)
        {
            UserViewModel user = await GetCurrentUserAsync();

            if (user == null)
            {
                return HandleUserNotFoundResult();
            }

            await _exchangeRateService.UpdateExchangeRateAsync(model);

            return true;
        }
    }
}
