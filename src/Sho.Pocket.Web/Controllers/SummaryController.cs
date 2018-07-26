using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Sho.Pocket.Core.Entities;
using Sho.Pocket.Core.Services;

namespace Sho.Pocket.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SummaryController : ControllerBase
    {
        private readonly ISummaryService _summaryService;

        public SummaryController(ISummaryService summaryService)
        {
            _summaryService = summaryService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PeriodSummary>> Get()
        {
            IEnumerable<PeriodSummary> summaries = _summaryService.GetPeriods();

            return new ActionResult<IEnumerable<PeriodSummary>>(summaries);
        }

        [HttpGet("{id}")]
        public ActionResult<PeriodSummary> Get(Guid id)
        {
            PeriodSummary summary = _summaryService.GetPeriod(id);

            return new ActionResult<PeriodSummary>(summary);
        }

        [HttpPost]
        public ActionResult<PeriodSummary> Post([FromBody] PeriodSummary period)
        {
            PeriodSummary newPeriod = _summaryService.AddPeriod(period);

            return new ActionResult<PeriodSummary>(newPeriod);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] PeriodSummary period)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _summaryService.DeletePeriod(id);
        }
    }
}
