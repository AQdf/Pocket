using System;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Core.Features.Assets.Abstractions;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Contexts
{
    [Binding]
    public class AssetFeatureContext : PocketFeatureBaseContext
    {
        public IAssetService AssetService;

        public AssetFeatureContext() : base()
        {
            AssetService = Services.GetRequiredService<IAssetService>();
        }
    }
}
