using System;
using System.Collections.Generic;
using System.Linq;
using Sho.Pocket.Core.Abstractions;
using Sho.Pocket.Core.Entities;
using Sho.Pocket.Data.Repositories;

namespace Sho.Pocket.BLL.Services
{
    public class SummaryService : ISummaryService
    {
        #region Dependencies

        private readonly AssetRepository _assetRepository;
        private readonly PeriodSummaryRepository _periodSummaryRepository;

        #endregion Dependencies

        #region Constructors

        public SummaryService(IDbConfiguration dbConfig)
        {
            _assetRepository = new AssetRepository();
            _periodSummaryRepository = new PeriodSummaryRepository();
        }

        #endregion Constructors

        #region ISummaryService implementation

        public List<PeriodSummary> GetPeriods()
        {
            bool includeAssets = true;

            List<PeriodSummary> result = _periodSummaryRepository.ReadAll();
            
            if (includeAssets)
            {
                PopulateAssets(result);
            }

            return result;
        }
        
        public PeriodSummary AddPeriod(PeriodSummary summary)
        {
            return _periodSummaryRepository.AddPeriod(summary);
        }

        public void DeletePeriod(Guid periodId)
        {
            PeriodSummaryRepository repository = new PeriodSummaryRepository();

            _periodSummaryRepository.DeletePeriod(periodId);
        }

        public Asset AddAssetToPeriod(Asset asset)
        {
            Asset result = _assetRepository.AddAsset(asset);

            _periodSummaryRepository.UpdatePeriodSummaryTotals(asset.PeriodId);
            
            return result;
        }

        public void UpdateAsset(Asset asset)
        {
            _assetRepository.UpdateAsset(asset);

            _periodSummaryRepository.UpdatePeriodSummaryTotals(asset.PeriodId);
        }

        public void RemoveAsset(Guid assetId, Guid periodId)
        {
            _assetRepository.RemoveAsset(assetId, periodId);

            _periodSummaryRepository.UpdatePeriodSummaryTotals(periodId);
        }

        #endregion ISummaryService implementation

        #region Private methods

        private void PopulateAssets(List<PeriodSummary> periods)
        {
            var assets = _assetRepository.GetAllAssets();

            foreach (PeriodSummary period in periods)
            {
                period.Assets = assets.Where(a => a.PeriodId == period.Id).ToList();
            }
        }

        #endregion Private methods
    }
}
