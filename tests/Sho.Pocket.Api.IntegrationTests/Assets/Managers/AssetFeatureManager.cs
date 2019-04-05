using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Application.Assets;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Api.IntegrationTests.Assets.Managers
{
    public class AssetFeatureManager : FeatureManagerBase
    {
        public Dictionary<Guid, Asset> Assets { get; set; } = new Dictionary<Guid, Asset>();

        public Dictionary<string, Currency> Currencies { get; set; } = new Dictionary<string, Currency>();

        public Dictionary<Guid, Balance> Balances { get; set; } = new Dictionary<Guid, Balance>();

        private readonly IAssetService _assetService;

        private readonly ICurrencyRepository _currencyRepository;

        private readonly IExchangeRateRepository _exchangeRateRepository;

        private readonly IBalanceRepository _balanceRepository;

        public AssetFeatureManager() : base()
        {
            _assetService = _serviceProvider.GetRequiredService<IAssetService>();
            _currencyRepository = _serviceProvider.GetRequiredService<ICurrencyRepository>();
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

        public Currency AddCurrency(string currencyName)
        {
            if (!Currencies.ContainsKey(currencyName))
            {
                Currency currency = _currencyRepository.Add(currencyName);
                Currencies.Add(currencyName, currency);
            }

            Currency result = Currencies[currencyName];

            return result;
        }

        public void InsertAssetBalance(Guid assetId, Guid currencyId)
        {
            DateTime effectiveDate = DateTime.UtcNow;
            ExchangeRate exchangeRate = _exchangeRateRepository.Add(effectiveDate, currencyId, currencyId, 1.0M);
            Balance balance = _balanceRepository.Add(assetId, effectiveDate, 200M, exchangeRate.Id);

            Balances.Add(balance.Id, balance);
        }
    }
}
