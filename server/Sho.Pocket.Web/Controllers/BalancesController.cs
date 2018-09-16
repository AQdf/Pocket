using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.Balances;
using Sho.Pocket.Application.Balances.Models;
using System;
using System.Collections.Generic;

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
        public string Get(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// POST: api/balances
        /// </summary>
        /// <param name="balanceModel"></param>
        /// <returns></returns>
        [HttpPost]
        public void Post([FromBody] BalanceViewModel balanceModel)
        {
            _balanceService.Add(balanceModel);
        }

        /// <summary>
        /// PUT: api/balances/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="balanceModel"></param>
        [HttpPut("{Id}")]
        public bool Put(Guid id, [FromBody] BalanceViewModel balanceModel)
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
        public decimal GetTotalBalance()
        {
            return _balanceService.GetTotalBalance();
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
    }
}