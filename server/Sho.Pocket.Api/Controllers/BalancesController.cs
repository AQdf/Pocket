using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.Balances;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Application.ExchangeRates.Models;

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
        [HttpGet]
        public BalancesViewModel GetAll([FromQuery] DateTime? effectiveDate)
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
        public bool Add([FromBody] BalanceViewModel balanceModel)
        {
            _balanceService.Add(balanceModel);

            return true;
        }

        /// <summary>
        /// PUT: api/balances/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="balanceModel"></param>
        [HttpPut("{id}")]
        public bool Update(Guid id, [FromBody] BalanceViewModel balanceModel)
        {
            _balanceService.Update(balanceModel);

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
        public decimal GetCurrentTotalBalance()
        {
            return _balanceService.GetCurrentTotalBalance();
        }

        /// <summary>
        /// POST: api/balances/template
        /// </summary>
        /// <param name="balanceModel"></param>
        /// <returns></returns>
        [HttpPost("template")]
        public bool AddBalancesTemplate()
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
    }
}