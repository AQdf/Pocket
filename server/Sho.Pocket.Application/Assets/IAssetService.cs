using System;
using System.Collections.Generic;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Application.AssetTypes.Models;
using Sho.Pocket.Application.Currencies.Models;

namespace Sho.Pocket.Application.Assets
{
    public interface IAssetService
    {
        IEnumerable<AssetViewModel> GetAll();

        void Add(AssetViewModel assetModel);

        void Update(AssetViewModel assetModel);

        void Delete(Guid Id, bool deactivate = true);

        List<AssetTypeViewModel> GetAssetTypes();

        List<CurrencyViewModel> GetCurrencies();
    }
}
