using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Core.Features.Currencies.Abstractions;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/currencies")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrenciesController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        /// <summary>
        /// GET: api/currencies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<string>> GetCurrencies()
        {
            List<string> result = await _currencyService.GetCurrenciesAsync();

            return result;
        }
    }
}
