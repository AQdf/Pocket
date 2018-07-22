using System;
using Sho.Pocket.Core.Entities;
using System.Collections.Generic;

namespace Sho.Pocket.Core.Abstractions
{
    public interface IAssetRepository
    {
        List<Asset> GetAllAssets();

        Asset AddAsset(Asset asset);

        void UpdateAsset(Asset asset);

        void RemoveAsset(Guid assetId, Guid periodId);
    }
}
