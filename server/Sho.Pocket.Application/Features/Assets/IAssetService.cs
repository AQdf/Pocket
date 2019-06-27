using Sho.Pocket.Application.Assets.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Application.Assets
{
    public interface IAssetService
    {
        Task<List<AssetViewModel>> GetAssetsAsync(Guid userOpenId);

        Task<AssetViewModel> GetAssetAsync(Guid userOpenId, Guid id);

        Task<AssetViewModel> AddAssetAsync(Guid userOpenId, AssetCreateModel createModel);

        Task<AssetViewModel> UpdateAsync(Guid userOpenId, Guid id, AssetUpdateModel updateModel);

        Task<bool> DeleteAsync(Guid userOpenId, Guid id);
    }
}
