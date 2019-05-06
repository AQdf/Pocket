using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Application.Currencies.Models;

namespace Sho.Pocket.Application.Assets
{
    public interface IAssetService
    {
        Task<IEnumerable<AssetViewModel>> GetAll();

        Task<AssetViewModel> GetById(Guid id);

        Task<AssetViewModel> Add(AssetCreateModel createModel);

        Task<AssetViewModel> Update(Guid id, AssetUpdateModel updateModel);

        Task<bool> Delete(Guid Id);

        Task<IEnumerable<CurrencyViewModel>> GetCurrencies();
    }
}
