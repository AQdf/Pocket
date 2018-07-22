using Sho.Pocket.Core.Entities;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Core.Abstractions
{
    public interface IPeriodSummaryRepository
    {
        List<PeriodSummary> ReadAll();

        PeriodSummary GetPeriod(Guid id);

        PeriodSummary AddPeriod(PeriodSummary summary);

        void DeletePeriod(Guid periodId);

        void UpdatePeriodSummaryTotals(Guid periodId);
    }
}
