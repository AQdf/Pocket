using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IAssetRepository
    {
        Task<Asset> GetById(Guid id);

        Task<IEnumerable<Asset>> GetAll();

        Task<IEnumerable<Asset>> GetActiveAssets();

        Task<Asset> Add(string name, Guid currencyId, bool isActive);

        Task<Asset> Update(Guid id, string name, Guid currencyId, bool isActive);

        Task Remove(Guid assetId);

        Task<bool> ExistsAssetBalance(Guid id);
    }
}
