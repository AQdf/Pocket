using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Application.Assets.Models;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Api.Controllers
{
    [Route("api/balances")]
    [ApiController]
    public class BalancesController : ControllerBase
    {
        public BalancesController()
        {
        }

        /// <summary>
        /// GET: api/balances
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<BalanceViewModel> GetAll()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// PUT: api/balances/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="balanceModel"></param>
        [HttpPut("{Id}")]
        public void Put(Guid id, [FromBody] BalanceViewModel balanceModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// DELETE: api/balances/0E056948-4014-4A2A-A132-5493A8499B9A
        /// </summary>
        /// <param name="Id"></param>
        [HttpDelete("{Id}")]
        public void Delete(Guid Id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// GET: api/balances/total
        /// </summary>
        /// <returns></returns>
        [HttpGet("total")]
        public decimal GetTotalBalance()
        {
            throw new NotImplementedException();
        }
    }
}