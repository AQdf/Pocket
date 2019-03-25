using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Application.Assets;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Core.DataAccess;

namespace Sho.Pocket.Api.IntegrationTests.Features
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

        public void InsertAssetToStorage(AssetCreateModel model)
        {
            var currency = _currencyRepository.Add("USD");
            model.CurrencyId = currency.Id;
            _assetService.Add(model);
        }

        public void CheckResult(bool actual)
        {
            var expected = true;

            var result = _assetService.GetAll();

            actual.Should().Be(expected);
        }
    }
}
