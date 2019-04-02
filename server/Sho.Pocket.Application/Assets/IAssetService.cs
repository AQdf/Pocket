using System;
using System.Collections.Generic;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Application.Currencies.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Assets
{
    public interface IAssetService
    {
        List<AssetViewModel> GetAll();

        Asset Add(AssetCreateModel createModel);

        Asset Update(Guid id, AssetUpdateModel updateModel);

        bool Delete(Guid Id);

        List<CurrencyViewModel> GetCurrencies();
    }
}
