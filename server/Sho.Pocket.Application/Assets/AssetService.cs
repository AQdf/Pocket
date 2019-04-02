using System;
using System.Collections.Generic;
using System.Linq;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Application.Currencies.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Assets
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;

        private readonly ICurrencyRepository _currencyRepository;

        public AssetService(
            IAssetRepository assetRepository,
            ICurrencyRepository currencyRepository)
        {
            _assetRepository = assetRepository;
            _currencyRepository = currencyRepository;
        }

        public List<AssetViewModel> GetAll()
        {
            List<Asset> assets = _assetRepository.GetAll();

            List<AssetViewModel> result = assets.Select(a => new AssetViewModel(a)).ToList();

            return result;
        }

        public Asset Add(AssetCreateModel createModel)
        {
            Asset asset = _assetRepository.Add(createModel.Name, createModel.CurrencyId, createModel.IsActive);

            return asset;
        }

        public Asset Update(Guid id, AssetUpdateModel model)
        {
            Asset asset = _assetRepository.Update(id, model.Name, model.IsActive);

            return asset;
        }

        public bool Delete(Guid id)
        {
            bool isSuccess = false;
            bool exists = _assetRepository.ExistsAssetBalance(id);

            if (!exists)
            {
                _assetRepository.Remove(id);
                isSuccess = true;
            }

            return isSuccess;
        }

        public List<CurrencyViewModel> GetCurrencies()
        {
            List<Currency> currencies = _currencyRepository.GetAll();

            List<CurrencyViewModel> result = currencies.Select(c => new CurrencyViewModel(c)).ToList();

            return result;
        }
    }
}
