using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.Balances;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/balances")]
    [ApiController]
    public class BalancesController : ControllerBase
    {
        private readonly IBalanceService _balanceService;

        public BalancesController(IBalanceService balanceService)
        {
            _balanceService = balanceService;
        }

        /// <summary>
        /// GET: api/balances
        /// </summary>
        /// <returns></returns>
        [HttpGet("{effectiveDate}")]
        public async Task<BalancesViewModel> GetAll(DateTime effectiveDate)
        {
            BalancesViewModel result = await _balanceService.GetAll(effectiveDate);

            return result;
        }

        /// <summary>
        /// GET: api/balances/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<BalanceViewModel> Get(Guid id)
        {
            BalanceViewModel balance = await _balanceService.GetById(id);

            return balance;
        }

        /// <summary>
        /// POST: api/balances
        /// </summary>
        /// <param name="balanceModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Add([FromBody] BalanceCreateModel createModel)
        {
            await _balanceService.Add(createModel);

            return true;
        }

        /// <summary>
        /// PUT: api/balances/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="balanceModel"></param>
        [HttpPut("{id}")]
        public async Task<bool> Update(Guid id, [FromBody] BalanceUpdateModel updateModel)
        {
            await _balanceService.Update(id, updateModel);

            return true;
        }

        /// <summary>
        /// DELETE: api/balances/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="Id"></param>
        [HttpDelete("{Id}")]
        public async Task<bool> Delete(Guid Id)
        {
            await _balanceService.Delete(Id);

            return true;
        }

        /// <summary>
        /// GET: api/balances/total
        /// </summary>
        /// <returns></returns>
        [HttpGet("total")]
        public async Task<IEnumerable<BalanceTotalModel>> GetCurrentTotalBalance()
        {
            IEnumerable<BalanceTotalModel> result = await _balanceService.GetCurrentTotalBalance();

            return result;
        }

        /// <summary>
        /// POST: api/balances/template
        /// </summary>
        /// <param name="balanceModel"></param>
        /// <returns></returns>
        [HttpPost("template")]
        public async Task<IEnumerable<BalanceViewModel>> AddBalancesTemplate()
        {
            IEnumerable<BalanceViewModel> result = await _balanceService.AddEffectiveBalancesTemplate();

            return result;
        }

        /// <summary>
        /// GET: api/balances/effective-dates
        /// </summary>
        /// <returns></returns>
        [HttpGet("effective-dates")]
        public async Task<IEnumerable<DateTime>> GetEffectiveDates()
        {
            IEnumerable<DateTime> result = await _balanceService.GetEffectiveDates();

            return result;
        }

        [HttpPut("exchange-rate")]
        public async Task<bool> ApplyExchangeRate([FromBody]ExchangeRateModel model)
        {
            await _balanceService.ApplyExchangeRate(model);

            return true;
        }

        [HttpGet("currency-totals/{currencyName}")]
        public async Task<IEnumerable<BalanceTotalModel>> GetCurrencyTotals(string currencyName, [FromQuery] int count = 10)
        {
            IEnumerable<BalanceTotalModel> result = await _balanceService.GetCurrencyTotals(currencyName, count);

            return result;
        }

        [HttpGet("csv")]
        public async Task<IActionResult> DownloadCsv()
        {
            byte[] bytes = await _balanceService.ExportBalancesToCsv();

            return File(bytes, "application/csv");
        }
    }
}