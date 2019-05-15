using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Assets
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;

        public AssetService(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<List<AssetViewModel>> GetAssetsAsync(Guid userOpenId)
        {
            IEnumerable<Asset> assets = await _assetRepository.GetByUserIdAsync(userOpenId);
            List<AssetViewModel> result = assets?.Select(a => new AssetViewModel(a)).ToList();

            return result;
        }

        public async Task<AssetViewModel> GetAssetAsync(Guid userOpenId, Guid id)
        {
            Asset asset = await _assetRepository.GetByIdAsync(userOpenId, id);
            AssetViewModel model = new AssetViewModel(asset);

            return model;
        }

        public async Task<AssetViewModel> AddAssetAsync(Guid userOpenId, AssetCreateModel createModel)
        {
            Asset asset = await _assetRepository.CreateAsync(userOpenId, createModel.Name, createModel.Currency, createModel.IsActive);
            AssetViewModel result = new AssetViewModel(asset);

            return result;
        }

        public async Task<AssetViewModel> UpdateAsync(Guid userOpenId, Guid id, AssetUpdateModel model)
        {
            bool balanceExists = await _assetRepository.ExistsAssetBalanceAsync(id);

            if (balanceExists)
            {
                Asset asset = await _assetRepository.GetByIdAsync(userOpenId, id);

                if (asset.Currency != model.Currency)
                {
                    return null;
                }
            }

            Asset result = await _assetRepository.UpdateAsync(userOpenId, id, model.Name, model.Currency, model.IsActive);
            AssetViewModel viewModel = new AssetViewModel(result);

            return viewModel;
        }

        public async Task<bool> DeleteAsync(Guid userOpenId, Guid id)
        {
            bool isSuccess = false;
            bool exists = await _assetRepository.ExistsAssetBalanceAsync(id);

            if (!exists)
            {
                await _assetRepository.RemoveAsync(userOpenId, id);
                isSuccess = true;
            }

            return isSuccess;
        }
    }
}
