using System;
using System.Collections.Generic;
using Sho.Pocket.Core.Entities;
using Sho.Pocket.Core.Repositories;
using Sho.Pocket.Core.Services;

namespace Sho.Pocket.BLL.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;

        public AssetService(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public List<Asset> GetAll()
        {
            return _assetRepository.GetAll();
        }

        public Asset Add(Asset asset)
        {
            Asset result = _assetRepository.Add(asset);

            return result;
        }

        public void Update(Asset asset)
        {
            _assetRepository.Update(asset);
        }

        public void Delete(Guid Id)
        {
            _assetRepository.Remove(Id);
        }
    }
}
