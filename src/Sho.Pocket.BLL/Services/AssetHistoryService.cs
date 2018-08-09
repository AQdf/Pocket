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

        public AssetHistory Add(AssetHistory assetHistory)
        {
            AssetHistory newAssetHistory = _assetHistoryRepository.Add(assetHistory);

            return newAssetHistory;
        }
    }
}
