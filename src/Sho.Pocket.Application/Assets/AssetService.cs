using System;
using System.Collections.Generic;
using System.Linq;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Application.AssetTypes.Models;
using Sho.Pocket.Application.Currencies.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Assets
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IAssetTypeRepository _assetTypeRepository;
        private readonly ICurrencyRepository _currencyRepository;

        public AssetService(
            IAssetRepository assetRepository,
            IAssetTypeRepository assetTypeRepository,
            ICurrencyRepository currencyRepository)
        {
            _assetRepository = assetRepository;
            _assetTypeRepository = assetTypeRepository;
            _currencyRepository = currencyRepository;
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

        List<AssetTypeViewModel> IAssetService.GetAssetTypes()
        {
            List<AssetType> assetTypes = _assetTypeRepository.GetAll();

            List<AssetTypeViewModel> result = assetTypes.Select(t => new AssetTypeViewModel(t.Id, t.Name)).ToList();

            return result;
        }

        public List<CurrencyViewModel> GetCurrencies()
        {
            List<Currency> currencies = _currencyRepository.GetAll();

            List<CurrencyViewModel> result = currencies.Select(c => new CurrencyViewModel(c.Id, c.Name)).ToList();

            return result;
        }
    }
}
