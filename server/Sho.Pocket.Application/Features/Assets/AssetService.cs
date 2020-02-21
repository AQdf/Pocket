using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.Assets.Abstractions;
using Sho.Pocket.Core.Features.Assets.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Assets
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;

        private readonly IBalanceRepository _balanceRepository;

        private readonly IUnitOfWork _unitOfWork;

        public AssetService(
            IAssetRepository assetRepository,
            IBalanceRepository balanceRepository,
            IUnitOfWork unitOfWork)
        {
            _assetRepository = assetRepository;
            _balanceRepository = balanceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AssetViewModel>> GetAssetsAsync(Guid userOpenId, bool includeInactive)
        {
            IEnumerable<Asset> assets = await _assetRepository.GetByUserIdAsync(userOpenId, includeInactive);
            List<AssetViewModel> result = assets.Select(a => new AssetViewModel(a)).ToList();

            return result;
        }

        public async Task<AssetViewModel> GetAssetAsync(Guid userOpenId, Guid id)
        {
            Asset asset = await _assetRepository.GetByIdAsync(userOpenId, id);

            if (asset == null)
            {
                throw new Exception($"Asset {id} not found!");
            }

            AssetViewModel result = new AssetViewModel(asset);

            return result;
        }

        public async Task<AssetViewModel> AddAssetAsync(Guid userOpenId, AssetCreateModel createModel)
        {
            Asset asset = await _assetRepository.CreateAsync(userOpenId, createModel.Name, createModel.Currency, createModel.IsActive);
            await _unitOfWork.SaveChangesAsync(); 
            AssetViewModel result = new AssetViewModel(asset);

            return result;
        }

        public async Task<AssetViewModel> UpdateAsync(Guid userOpenId, Guid id, AssetUpdateModel model)
        {
            Asset asset = await _assetRepository.GetByIdAsync(userOpenId, id);

            if (asset == null)
            {
                throw new Exception($"Asset {id} not found!");
            }

            bool balanceExists = await _balanceRepository.ExistsAssetBalanceAsync(id);

            if (balanceExists && asset.Currency != model.Currency)
            {
                throw new Exception($"Can't update currency of the Asset {id} if Asset's Balance exists!");
            }

            Asset result = await _assetRepository.UpdateAsync(userOpenId, id, model.Name, model.Currency, model.IsActive);
            await _unitOfWork.SaveChangesAsync();

            AssetViewModel viewModel = new AssetViewModel(result);

            return viewModel;
        }

        public async Task<bool> DeleteAsync(Guid userOpenId, Guid id)
        {
            bool isSuccess = false;
            bool exists = await _balanceRepository.ExistsAssetBalanceAsync(id);

            if (!exists)
            {
                await _assetRepository.RemoveAsync(userOpenId, id);
                await _unitOfWork.SaveChangesAsync();

                isSuccess = true;
            }

            return isSuccess;
        }
    }
}
