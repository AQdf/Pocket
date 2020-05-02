using Moq;
using Sho.Pocket.Application.Assets;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.Assets.Abstractions;
using Sho.Pocket.Core.Features.Assets.Models;
using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sho.Pocket.Application.UnitTests.Assets
{
    public class AssetServiceTest
    {
        private readonly IAssetService assetService;

        private readonly Mock<IAssetRepository> assetRepositoryMock;

        private readonly Guid currentUserId = Guid.NewGuid();

        public AssetServiceTest()
        {
            assetRepositoryMock = new Mock<IAssetRepository>();
            Mock<IBalanceRepository> balanceRepositoryMock = new Mock<IBalanceRepository>();
            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();

            assetService = new AssetService(assetRepositoryMock.Object, balanceRepositoryMock.Object, unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetAssetsAsync_MappedResultFromDbReturned()
        {
            List<Asset> assets = new List<Asset>
            {
                new Asset(Guid.NewGuid(), "Cash inactive", "USD", false, currentUserId, 30.0M, new DateTime(2019, 4, 21)),
                new Asset(Guid.NewGuid(), "Cash active", "USD", true, currentUserId, 30.0M, new DateTime(2019, 4, 21)),
                new Asset(Guid.NewGuid(), "Bank account active", "USD", true, currentUserId, 40.0M, new DateTime(2019, 8, 25))
            };

            assetRepositoryMock.Setup(m => m.GetByUserIdAsync(currentUserId, true)).ReturnsAsync(assets);

            List<AssetViewModel> expected = assets.Select(a => new AssetViewModel(a)).ToList();

            List<AssetViewModel> actual = await assetService.GetAssetsAsync(currentUserId, true);

            Assert.Equal(expected, actual);
        }
    }
}
