using System;
using System.Collections.Generic;
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
        public BalancesViewModel GetAll(DateTime effectiveDate)
        {
            return _balanceService.GetAll(effectiveDate);
        }

        /// <summary>
        /// GET: api/balances/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public BalanceViewModel Get(Guid id)
        {
            BalanceViewModel balance = _balanceService.GetById(id);

            return balance;
        }

        /// <summary>
        /// POST: api/balances
        /// </summary>
        /// <param name="balanceModel"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Add([FromBody] BalanceCreateModel createModel)
        {
            _balanceService.Add(createModel);

            return true;
        }

        /// <summary>
        /// PUT: api/balances/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="balanceModel"></param>
        [HttpPut("{id}")]
        public bool Update(Guid id, [FromBody] BalanceUpdateModel updateModel)
        {
            _balanceService.Update(id, updateModel);

            return true;
        }

        /// <summary>
        /// DELETE: api/balances/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="Id"></param>
        [HttpDelete("{Id}")]
        public bool Delete(Guid Id)
        {
            _balanceService.Delete(Id);

            return true;
        }

        /// <summary>
        /// GET: api/balances/total
        /// </summary>
        /// <returns></returns>
        [HttpGet("total")]
        public IEnumerable<BalanceTotalModel> GetCurrentTotalBalance()
        {
            return _balanceService.GetCurrentTotalBalance();
        }

        /// <summary>
        /// POST: api/balances/template
        /// </summary>
        /// <param name="balanceModel"></param>
        /// <returns></returns>
        [HttpPost("template")]
        public IEnumerable<Balance> AddBalancesTemplate()
        {
            return _balanceService.AddEffectiveBalancesTemplate();
        }

        /// <summary>
        /// GET: api/balances/effective-dates
        /// </summary>
        /// <returns></returns>
        [HttpGet("effective-dates")]
        public IEnumerable<DateTime> GetEffectiveDates()
        {
            return _balanceService.GetEffectiveDates();
        }

        [HttpPut("exchange-rate")]
        public bool ApplyExchangeRate([FromBody]ExchangeRateModel model)
        {
            _balanceService.ApplyExchangeRate(model);

            return true;
        }

        [HttpGet("currency-totals/{currencyId}")]
        public IEnumerable<BalanceTotalModel> GetCurrencyTotals(Guid currencyId, [FromQuery] int count = 10)
        {
            var result = _balanceService.GetCurrencyTotals(currencyId, count);

            return result;
        }

        [HttpGet("csv")]
        public IActionResult DownloadCsv()
        {
            byte[] bytes = _balanceService.ExportBalancesToCsv();

            return File(bytes, "application/csv");
        }
    }
}