using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Api.IntegrationTests.Currencies.Managers;
using Sho.Pocket.Application.Assets;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Api.IntegrationTests.Assets.Managers
{
    public class AssetFeatureManager : FeatureManagerBase
    {
        public Dictionary<Guid, Asset> Assets { get; set; } = new Dictionary<Guid, Asset>();

        private readonly IAssetService _assetService;

        private readonly CurrencyFeatureManager _currencyFeatureManager;

        private readonly IExchangeRateRepository _exchangeRateRepository;

        private readonly IBalanceRepository _balanceRepository;

        public AssetFeatureManager(CurrencyFeatureManager currencyFeatureManager) : base()
        {
            _currencyFeatureManager = currencyFeatureManager;

            _assetService = _serviceProvider.GetRequiredService<IAssetService>();
            _exchangeRateRepository = _serviceProvider.GetRequiredService<IExchangeRateRepository>();
            _balanceRepository = _serviceProvider.GetRequiredService<IBalanceRepository>();
        }

        public List<AssetViewModel> GetAssets()
        {
            List<AssetViewModel> storageAssets = _assetService.GetAll();

            List<AssetViewModel> contextAssets = storageAssets.Where(sa => Assets.ContainsKey(sa.Id.Value)).ToList();

            return contextAssets;
        }

        public Asset AddAsset(AssetCreateModel createModel)
        {
            Asset asset = _assetService.Add(createModel);
            Assets.Add(asset.Id, asset);

            return asset;
        }

        public void DeleteAsset(Guid id)
        {
            bool isSuccess = _assetService.Delete(id);

            if (isSuccess)
            {
                Assets.Remove(id);
            }
        }

        public Asset UpdateAsset(Guid id, AssetUpdateModel updateModel)
        {
            Asset result = _assetService.Update(id, updateModel);
            Assets[id] = result;

            return result;
        }
    }
}
