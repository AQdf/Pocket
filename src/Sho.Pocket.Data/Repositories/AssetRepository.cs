using Sho.Pocket.Core.Abstractions;
using Sho.Pocket.Core.Entities;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Data.Repositories
{
    public class AssetRepository : BaseRepository<Asset>, IAssetRepository
    {
        private const string SCRIPTS_DIR_NAME = "Asset";

        public AssetRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public List<Asset> GetAllAssets()
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "ReadAllAssets.sql");

            List<Asset> result = base.GetAll(queryText);

            return result;
        }

        public Asset AddAsset(Asset asset)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "InsertAsset.sql");

            object queryParameters = new
            {
                name = asset.Name,
                typeName = asset.TypeName,
                currencyName = asset.CurrencyName,
                balance = asset.Balance,
                periodId = asset.PeriodId
            };

            Asset result = base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public void UpdateAsset(Asset asset)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "UpdateAsset.sql");

            object queryParameters = new
            {
                id = asset.Id,
                name = asset.Name,
                typeName = asset.TypeName,
                currencyName = asset.CurrencyName,
                balance = asset.Balance,
                periodId = asset.PeriodId
            };

            base.UpdateEntity(queryText, queryParameters);
        }

        public void RemoveAsset(Guid assetId, Guid periodId)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "DeleteAsset.sql");

            object queryParameters = new
            {
                id = assetId
            };

            base.RemoveEntity(queryText, queryParameters);
        }
    }
}
