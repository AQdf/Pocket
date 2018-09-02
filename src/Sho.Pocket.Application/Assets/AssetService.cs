using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<AssetViewModel> GetAll()
        {
            List<Asset> assets = _assetRepository.GetAll();

            List<AssetViewModel> result = assets.Select(a => new AssetViewModel(a)).ToList();

            return result;
        }

        public void Add(AssetViewModel assetModel)
        {
            Asset asset = new Asset
            {
                Name = assetModel.Name,
                TypeId = assetModel.TypeId,
                CurrencyId = assetModel.CurrencyId,
                IsActive = assetModel.IsActive
            };

            _assetRepository.Add(asset);
        }

        public void Update(AssetViewModel assetModel)
        {
            Asset asset = new Asset
            {
                Id = assetModel.Id,
                Name = assetModel.Name,
                TypeId = assetModel.TypeId,
                CurrencyId = assetModel.CurrencyId,
                IsActive = assetModel.IsActive
            };

            _assetRepository.Update(asset);
        }

        public void Delete(Guid Id, bool deactivate = true)
        {
            if (deactivate)
            {
                _assetRepository.DeactivateAsset(Id);
            }
            else
            {
                _assetRepository.Remove(Id);
            }
        }
    }
}
