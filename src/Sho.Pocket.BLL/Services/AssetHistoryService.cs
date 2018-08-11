using System;
using System.Collections.Generic;
using Sho.Pocket.Core.Entities;
using Sho.Pocket.Core.Repositories;
using Sho.Pocket.Core.Services;

namespace Sho.Pocket.BLL.Services
{
    public class AssetHistoryService : IAssetHistoryService
    {
        private readonly IAssetHistoryRepository _assetHistoryRepository;

        public AssetHistoryService(IAssetHistoryRepository assetHistoryRepository)
        {
            _assetHistoryRepository = assetHistoryRepository;
        }

        public List<AssetHistory> GetAll()
        {
            return _assetHistoryRepository.GetAll();
        }

        public AssetHistory Add(AssetHistory assetHistory)
        {
            AssetHistory newAssetHistory = _assetHistoryRepository.Add(assetHistory);

            return newAssetHistory;
        }

        public void Delete(Guid id)
        {
            _assetHistoryRepository.Remove(id);
        }

        public void Update(AssetHistory assetHistory)
        {
            _assetHistoryRepository.Update(assetHistory);
        }
    }
}
