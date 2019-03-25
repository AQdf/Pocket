using System;
using System.Collections.Generic;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Application.Currencies.Models;

namespace Sho.Pocket.Application.Assets
{
    public interface IAssetService
    {
        IEnumerable<AssetViewModel> GetAll();

        void Add(AssetCreateModel createModel);

        void Update(AssetViewModel assetModel);

        void Delete(Guid Id);

        List<CurrencyViewModel> GetCurrencies();
    }
}
