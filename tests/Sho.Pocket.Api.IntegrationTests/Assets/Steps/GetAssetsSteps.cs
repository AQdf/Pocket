﻿using FluentAssertions;
using Sho.Pocket.Api.IntegrationTests.Assets.Managers;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Application.Assets.Models;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Assets.Steps
{
    [Binding]
    public class GetAssetsSteps
    {
        private List<AssetViewModel> _assets;

        private AssetFeatureManager _assetFeatureManager;

        public GetAssetsSteps(AssetFeatureManager assetFeatureManager)
        {
            _assetFeatureManager = assetFeatureManager;
        }

        [BeforeTestRun]
        public static void Cleanup()
        {
            StorageCleaner.Cleanup();
        }

        [When(@"I get assets")]
        public void WhenIGetAssets()
        {
            _assets = _assetFeatureManager.GetAssets();
        }
        
        [Then(@"my (.*) assets returned")]
        public void ThenMyAssetsReturned(int assetsCount)
        {
            _assets.Count.Should().Be(assetsCount);
        }
    }
}