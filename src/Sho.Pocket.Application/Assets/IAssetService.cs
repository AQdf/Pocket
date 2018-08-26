using System;
using System.Collections.Generic;
using Sho.Pocket.Application.Assets.Models;

namespace Sho.Pocket.Application.Assets
{
    public interface IAssetService
    {
        IEnumerable<AssetViewModel> GetAll();

        void Add(AssetViewModel assetModel);

        void Update(AssetViewModel assetModel);

        void Delete(Guid Id, bool deactivate = true);
    }
}
