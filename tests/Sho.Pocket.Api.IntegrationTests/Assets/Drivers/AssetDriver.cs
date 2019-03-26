using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Application.Assets;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Api.IntegrationTests.Assets
{
    internal class AssetDriver : FeatureDriverBase
    {
        private readonly ICurrencyRepository _currencyRepository;

        private readonly IExchangeRateRepository _exchangeRateRepository;

        private readonly IBalanceRepository _balanceRepository;

        private readonly IAssetService _assetService;

        public AssetDriver() : base()
        {
            _currencyRepository = _serviceProvider.GetRequiredService<ICurrencyRepository>();

            _exchangeRateRepository = _serviceProvider.GetRequiredService<IExchangeRateRepository>();

            _balanceRepository = _serviceProvider.GetRequiredService<IBalanceRepository>();

            _assetService = _serviceProvider.GetRequiredService<IAssetService>();
        }

        public Currency InsertCurrencyToStorage(string currencyName)
        {
            Currency currency = _currencyRepository.Add(currencyName);

            return currency;
        }

        public Asset InsertAssetToStorage(AssetCreateModel model)
        {
            Asset asset = _assetService.Add(model);

            return asset;
        }

        public void UpdateAssetInStorage(AssetViewModel model)
        {
            _assetService.Update(model);
        }

        public void InsertAssetBalance(Guid assetId, Guid currencyId)
        {
            DateTime effectiveDate = DateTime.UtcNow;

            ExchangeRate exchangeRate = _exchangeRateRepository.Add(effectiveDate, currencyId, currencyId, 1.0M);

            _balanceRepository.Add(assetId, effectiveDate, 200M, exchangeRate.Id);
        }

        public bool DeleteAssetFromStorage(Guid id)
        {
            var isSuccess =_assetService.Delete(id);

            return isSuccess;
        }

        public List<AssetViewModel> GetAssets()
        {
            const string query = @"
                select  Asset.Id as Id,
                        Asset.Name as Name,
                        Asset.IsActive as IsActive,
                        Asset.CurrencyId as CurrencyId,
                        Currency.Name as CurrencyName
                from Asset
                join Currency on Currency.Id = Asset.CurrencyId";

            List<AssetViewModel> assets = GetList<AssetViewModel>(query);

            return assets;
        }
    }
}
