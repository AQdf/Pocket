using Sho.Pocket.Core.Entities;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Core.Abstractions
{
    public interface ISummaryService
    {
        List<PeriodSummary> GetPeriods();

        PeriodSummary AddPeriod(PeriodSummary summary);

        Asset AddAssetToPeriod(Asset asset);

        void RemoveAsset(Guid assetId, Guid periodId);

        void UpdateAsset(Asset asset);

        void DeletePeriod(Guid periodId);
    }
}
