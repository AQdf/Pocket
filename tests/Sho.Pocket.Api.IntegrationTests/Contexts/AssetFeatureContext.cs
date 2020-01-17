using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Core.Features.Assets.Abstractions;
using Sho.Pocket.Core.Features.Assets.Models;

namespace Sho.Pocket.Api.IntegrationTests.Contexts
{
    public class AssetFeatureContext : FeatureContextBase
    {
        public Dictionary<string, AssetViewModel> Assets { get; set; }

        private readonly IAssetService _assetService;

        public AssetFeatureContext() : base()
        {
            Assets = new Dictionary<string, AssetViewModel>();

            _assetService = _serviceProvider.GetRequiredService<IAssetService>();
        }

        public async Task<List<AssetViewModel>> GetAssets()
        {
            IEnumerable<AssetViewModel> storageAssets = await _assetService.GetAssetsAsync(User.Id);

            List<AssetViewModel> contextAssets = storageAssets.Where(sa => Assets.ContainsKey(sa.Name)).ToList();

            return contextAssets;
        }

        public async Task<AssetViewModel> AddAsset(AssetCreateModel createModel)
        {
            AssetViewModel asset = await _assetService.AddAssetAsync(User.Id, createModel);
            Assets.Add(asset.Name, asset);

            return asset;
        }

        public async Task DeleteAsset(Guid id, string assetName)
        {
            bool isSuccess = await _assetService.DeleteAsync(User.Id, id);

            if (isSuccess)
            {
                Assets.Remove(assetName);
            }
        }

        public async Task<AssetViewModel> UpdateAsset(Guid id, AssetUpdateModel updateModel)
        {
            AssetViewModel result = await _assetService.UpdateAsync(User.Id, id, updateModel);
            Assets[updateModel.Name] = result;

            return result;
        }
    }
}
