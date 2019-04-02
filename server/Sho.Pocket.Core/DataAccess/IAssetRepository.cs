using System;
using System.Collections.Generic;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IAssetRepository
    {
        List<Asset> GetAll(bool includeRelated = true);

        Asset Add(string name, Guid currencyId, bool isActive);

        Asset Update(Guid id, string name, bool isActive);

        void Remove(Guid assetId);

        bool ExistsAssetBalance(Guid id);
    }
}
