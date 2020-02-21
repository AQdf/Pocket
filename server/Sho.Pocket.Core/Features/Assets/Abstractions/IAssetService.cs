using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.Assets.Models;

namespace Sho.Pocket.Core.Features.Assets.Abstractions
{
    public interface IAssetService
    {
        Task<List<AssetViewModel>> GetAssetsAsync(Guid userOpenId, bool includeInactive);

        Task<AssetViewModel> GetAssetAsync(Guid userOpenId, Guid id);

        Task<AssetViewModel> AddAssetAsync(Guid userOpenId, AssetCreateModel createModel);

        Task<AssetViewModel> UpdateAsync(Guid userOpenId, Guid id, AssetUpdateModel updateModel);

        Task<bool> DeleteAsync(Guid userOpenId, Guid id);
    }
}
