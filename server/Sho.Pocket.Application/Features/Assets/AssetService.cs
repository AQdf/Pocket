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

        public async Task<List<AssetViewModel>> GetAssetsAsync(Guid userId, bool includeInactive)
        {
            IEnumerable<Asset> assets = await _assetRepository.GetByUserIdAsync(userId, includeInactive);
            List<AssetViewModel> result = assets.Select(a => new AssetViewModel(a)).ToList();

            return result;
        }

        public async Task<AssetViewModel> GetAssetByIdAsync(Guid userId, Guid id)
        {
            Asset asset = await _assetRepository.GetByIdAsync(userId, id);

            if (asset == null)
            {
                throw new Exception($"Asset {id} not found!");
            }

            AssetViewModel result = new AssetViewModel(asset);

            return result;
        }

        public async Task<AssetViewModel> AddAssetAsync(Guid userId, AssetCreateModel createModel)
        {
            Asset asset = await _assetRepository.CreateAsync(
                userId, createModel.Name, createModel.Currency, createModel.IsActive, createModel.Value, DateTime.UtcNow);
            await _unitOfWork.SaveChangesAsync(); 
            AssetViewModel result = new AssetViewModel(asset);

            return result;
        }

        public async Task<AssetViewModel> UpdateAsync(Guid userId, Guid id, AssetUpdateModel model)
        {
            Asset asset = await _assetRepository.GetByIdAsync(userId, id);

            if (asset == null)
            {
                throw new Exception($"Asset {id} not found!");
            }

            bool balanceExists = await _balanceRepository.ExistsAssetBalanceAsync(userId, id);

            if (balanceExists && asset.Currency != model.Currency)
            {
                throw new Exception($"Can't update currency of the Asset {id} if Asset's Balance exists!");
            }

            Asset result = await _assetRepository.UpdateAsync(userId, id, model.Name, model.Currency, model.IsActive, model.Value, DateTime.UtcNow);
            await _unitOfWork.SaveChangesAsync();

            AssetViewModel viewModel = new AssetViewModel(result);

            return viewModel;
        }

        public async Task<bool> DeleteAsync(Guid userId, Guid id)
        {
            bool isSuccess = false;
            bool exists = await _balanceRepository.ExistsAssetBalanceAsync(userId, id);

            if (!exists)
            {
                await _assetRepository.RemoveAsync(userId, id);
                await _unitOfWork.SaveChangesAsync();

                isSuccess = true;
            }

            return isSuccess;
        }

        public async Task<AssetViewModel> GetAssetByNameAsync(Guid userId, string name)
        {
            Asset asset = await _assetRepository.GetByNameAsync(userId, name);

            return new AssetViewModel(asset);
        }
    }
}
