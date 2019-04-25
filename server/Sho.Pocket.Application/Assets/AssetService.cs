using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<AssetViewModel>> GetAll()
        {
            IEnumerable<Asset> assets = await _assetRepository.GetAll();

            IEnumerable<AssetViewModel> result = assets.Select(a => new AssetViewModel(a));

            return result;
        }

        public async Task<AssetViewModel> Add(AssetCreateModel createModel)
        {
            Asset asset = await _assetRepository.Add(createModel.Name, createModel.CurrencyId, createModel.IsActive);

            AssetViewModel result = new AssetViewModel(asset);

            return result;
        }

        public async Task<AssetViewModel> Update(Guid id, AssetUpdateModel model)
        {
            bool balanceExists = await _assetRepository.ExistsAssetBalance(id);

            if (balanceExists)
            {
                Asset asset = await _assetRepository.GetById(id);

                if (asset.CurrencyId != model.CurrencyId)
                {
                    return null;
                }
            }

            Asset result = await _assetRepository.Update(id, model.Name, model.CurrencyId, model.IsActive);

            AssetViewModel viewModel = new AssetViewModel(result);

            return viewModel;
        }

        public async Task<bool> Delete(Guid id)
        {
            bool isSuccess = false;
            bool exists = await _assetRepository.ExistsAssetBalance(id);

            if (!exists)
            {
                await _assetRepository.Remove(id);
                isSuccess = true;
            }

            return isSuccess;
        }

        public async Task<IEnumerable<CurrencyViewModel>> GetCurrencies()
        {
            IEnumerable<Currency> currencies = await _currencyRepository.GetAll();

            IEnumerable<CurrencyViewModel> result = currencies.Select(c => new CurrencyViewModel(c));

            return result;
        }
    }
}
