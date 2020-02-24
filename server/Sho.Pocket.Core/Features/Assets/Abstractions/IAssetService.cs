using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.Assets.Models;

namespace Sho.Pocket.Core.Features.Assets.Abstractions
{
    public interface IAssetService
    {
        Task<List<AssetViewModel>> GetAssetsAsync(Guid userId, bool includeInactive);

        Task<AssetViewModel> GetAssetAsync(Guid userId, Guid id);

        Task<AssetViewModel> AddAssetAsync(Guid userId, AssetCreateModel createModel);

        Task<AssetViewModel> UpdateAsync(Guid userId, Guid id, AssetUpdateModel updateModel);

        Task<bool> DeleteAsync(Guid userId, Guid id);
    }
}
