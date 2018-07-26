using System;
using System.Collections.Generic;
using Sho.Pocket.Core.Entities;
using Sho.Pocket.Core.Repositories;
using Sho.Pocket.Core.Services;

namespace Sho.Pocket.BLL.Services
{
    public class SummaryService : ISummaryService
    {
        #region Dependencies

        private readonly IPeriodSummaryRepository _periodSummaryRepository;
        private readonly IAssetRepository _assetRepository;

        #endregion Dependencies

        #region Constructors

        public SummaryService(IPeriodSummaryRepository periodSummaryRepository, IAssetRepository assetRepository)
        {
            _periodSummaryRepository = periodSummaryRepository;
            _assetRepository = assetRepository;
        }

        #endregion Constructors

        #region ISummaryService implementation

        public List<PeriodSummary> GetPeriods()
        {
            List<PeriodSummary> result = _periodSummaryRepository.ReadAll();

            return result;
        }

        public PeriodSummary GetPeriod(Guid id)
        {
            PeriodSummary result = _periodSummaryRepository.GetPeriod(id);

            return result;
        }

        public PeriodSummary AddPeriod(PeriodSummary summary)
        {
            return _periodSummaryRepository.AddPeriod(summary);
        }

        public void DeletePeriod(Guid periodId)
        {
            _periodSummaryRepository.DeletePeriod(periodId);
        }

        #endregion ISummaryService implementation
    }
}
