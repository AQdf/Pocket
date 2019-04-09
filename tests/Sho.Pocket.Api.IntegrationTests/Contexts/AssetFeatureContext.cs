using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Application.Assets;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Api.IntegrationTests.Contexts
{
    public class AssetFeatureContext : FeatureContextBase
    {
        public Dictionary<string, AssetViewModel> Assets { get; set; }

        private readonly IAssetService _assetService;

        private readonly CurrencyFeatureContext _currencyFeatureContext;

        private readonly IExchangeRateRepository _exchangeRateRepository;

        private readonly IBalanceRepository _balanceRepository;

        public AssetFeatureContext(CurrencyFeatureContext currencyFeatureManager) : base()
        {
            Assets = new Dictionary<string, AssetViewModel>();

            _currencyFeatureContext = currencyFeatureManager;

            _assetService = _serviceProvider.GetRequiredService<IAssetService>();
            _exchangeRateRepository = _serviceProvider.GetRequiredService<IExchangeRateRepository>();
            _balanceRepository = _serviceProvider.GetRequiredService<IBalanceRepository>();
        }

        public List<AssetViewModel> GetAssets()
        {
            List<AssetViewModel> storageAssets = _assetService.GetAll();

            List<AssetViewModel> contextAssets = storageAssets.Where(sa => Assets.ContainsKey(sa.Name)).ToList();

            return contextAssets;
        }

        public AssetViewModel AddAsset(AssetCreateModel createModel)
        {
            AssetViewModel asset = _assetService.Add(createModel);
            Assets.Add(asset.Name, asset);

            return asset;
        }

        public void DeleteAsset(Guid id, string assetName)
        {
            bool isSuccess = _assetService.Delete(id);

            if (isSuccess)
            {
                Assets.Remove(assetName);
            }
        }

        public AssetViewModel UpdateAsset(Guid id, AssetUpdateModel updateModel)
        {
            AssetViewModel result = _assetService.Update(id, updateModel);
            Assets[updateModel.Name] = result;

            return result;
        }
    }
}
