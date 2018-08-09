using Sho.Pocket.Core.Entities;
using System;
using System.Collections.Generic;


namespace Sho.Pocket.Core.Services
{
    public interface IAssetService
    {
        List<Asset> GetAll();

        Asset Add(Asset asset);

        void Update(Asset asset);

        void Delete(Guid Id, bool deactivate = true);
    }
}
