using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Application.Assets;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Api.IntegrationTests.Assets
{
    public class AssetDriver : TestDriverBase
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IAssetService _assetService;

        public AssetDriver() : base()
        {
            _currencyRepository = _serviceProvider.GetRequiredService<ICurrencyRepository>();
            _assetService = _serviceProvider.GetRequiredService<IAssetService>();
        }

        public Currency InsertCurrencyToStorage(string currencyName)
        {
            Currency currency = _currencyRepository.Add(currencyName);

            return currency;
        }

        public void InsertAssetToStorage(AssetCreateModel model)
        {
            _assetService.Add(model);
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

        public void Cleanup()
        {
            const string query = @"
                delete from Asset;
                delete from Currency;";

            ExecuteScript(query);
        }
    }
}
