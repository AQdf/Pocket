using Sho.Pocket.Core.Entities;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Core.Repositories
{
    public interface IAssetHistoryRepository
    {
        List<AssetHistory> GetAll();

        AssetHistory Add(AssetHistory assetHistory);

        void Update(AssetHistory assetHistory);

        void Remove(Guid assetHistoryId);
    }
}
