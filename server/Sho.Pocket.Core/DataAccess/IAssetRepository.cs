using System;
using System.Collections.Generic;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IAssetRepository
    {
        Asset GetById(Guid id);

        List<Asset> GetAll();

        List<Asset> GetActiveAssets();

        Asset Add(string name, Guid currencyId, bool isActive);

        Asset Update(Guid id, string name, Guid currencyId, bool isActive);

        void Remove(Guid assetId);

        bool ExistsAssetBalance(Guid id);
    }
}
