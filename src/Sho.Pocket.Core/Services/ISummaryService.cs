using Sho.Pocket.Core.Entities;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Core.Services
{
    public interface ISummaryService
    {
        List<PeriodSummary> GetPeriods();

        PeriodSummary GetPeriod(Guid id);

        PeriodSummary AddPeriod(PeriodSummary summary);

        void DeletePeriod(Guid periodId);
    }
}
