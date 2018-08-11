using Sho.Pocket.Core.Entities;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Core.Services
{
    public interface IAssetHistoryService
    {
        List<AssetHistory> GetAll();

        AssetHistory Add(AssetHistory assetHistory);

        void Update(AssetHistory assetHistory);

        void Delete(Guid id);
    }
}
