using System;
using System.Collections.Generic;
using Sho.Pocket.Core.Entities;
using Sho.Pocket.Core.Repositories;
using Sho.Pocket.Core.Services;

namespace Sho.Pocket.BLL.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IAssetHistoryRepository _assetHistoryRepository;

        public AssetService(IAssetRepository assetRepository, IAssetHistoryRepository assetHistoryRepository)
        {
            _assetRepository = assetRepository;
            _assetHistoryRepository = assetHistoryRepository;
        }

        public List<Asset> GetAll()
        {
            return _assetRepository.GetAll();
        }

        public Asset Add(Asset asset)
        {
            Asset addedAsset = _assetRepository.Add(asset);

            InsertAssetHistoryRecord(addedAsset);

            return addedAsset;
        }

        public void Update(Asset asset)
        {
            _assetRepository.Update(asset);

            InsertAssetHistoryRecord(asset);
        }

        public void Delete(Guid Id, bool deactivate = true)
        {
            if (deactivate)
            {
                _assetRepository.DeactivateAsset(Id);
            }
            else
            {
                _assetHistoryRepository.Remove(Id);
            }
        }

        private void InsertAssetHistoryRecord(Asset asset)
        {
            AssetHistory assetHistory = new AssetHistory(asset);
            _assetHistoryRepository.Add(assetHistory);
        }
    }
}
