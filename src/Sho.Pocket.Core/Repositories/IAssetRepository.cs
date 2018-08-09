using System;
using Sho.Pocket.Core.Entities;
using System.Collections.Generic;

namespace Sho.Pocket.Core.Repositories
{
    public interface IAssetRepository
    {
        List<Asset> GetAll();

        Asset Add(Asset asset);

        void Update(Asset asset);

        void Remove(Guid assetId);

        void DeactivateAsset(Guid assetId);
    }
}
