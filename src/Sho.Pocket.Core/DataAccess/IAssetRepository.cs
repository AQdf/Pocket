using System;
using System.Collections.Generic;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IAssetRepository
    {
        List<Asset> GetAll(bool includeRelated = true);

        Asset Add(Asset asset);

        void Update(Asset asset);

        void Remove(Guid assetId);

        void DeactivateAsset(Guid assetId);
    }
}
