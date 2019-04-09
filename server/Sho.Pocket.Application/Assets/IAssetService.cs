using System;
using System.Collections.Generic;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Application.Currencies.Models;

namespace Sho.Pocket.Application.Assets
{
    public interface IAssetService
    {
        List<AssetViewModel> GetAll();

        AssetViewModel Add(AssetCreateModel createModel);

        AssetViewModel Update(Guid id, AssetUpdateModel updateModel);

        bool Delete(Guid Id);

        List<CurrencyViewModel> GetCurrencies();
    }
}
