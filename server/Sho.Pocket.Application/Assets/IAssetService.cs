using System;
using System.Collections.Generic;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Application.Currencies.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Assets
{
    public interface IAssetService
    {
        IEnumerable<AssetViewModel> GetAll();

        Asset Add(AssetCreateModel createModel);

        void Update(AssetViewModel assetModel);

        bool Delete(Guid Id);

        List<CurrencyViewModel> GetCurrencies();
    }
}
