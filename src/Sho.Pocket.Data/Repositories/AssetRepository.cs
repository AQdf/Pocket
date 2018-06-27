using Sho.Pocket.Core.Entities;
using System;
using System.Collections.Generic;

namespace Sho.Pocket.Data.Repositories
{
    public class AssetRepository
    {
        private const string SCRIPTS_DIR_NAME = "Asset";

        public List<Asset> GetAllAssets()
        {
            string queryText = DbHelper.GetQueryText(SCRIPTS_DIR_NAME, "ReadAllAssets.sql");

            List<Asset> result = DbHelper.GetAll<Asset>(queryText);

            return result;
        }

        public Asset AddAsset(Asset asset)
        {
            string queryText = DbHelper.GetQueryText(SCRIPTS_DIR_NAME, "InsertAsset.sql");

            object queryParameters = new
            {
                name = asset.Name,
                typeName = asset.TypeName,
                currencyName = asset.CurrencyName,
                balance = asset.Balance,
                periodId = asset.PeriodId
            };

            Asset result = DbHelper.InsertEntity<Asset>(queryText, queryParameters);

            return result;
        }

        public void UpdateAsset(Asset asset)
        {
            string queryText = DbHelper.GetQueryText(SCRIPTS_DIR_NAME, "UpdateAsset.sql");

            object queryParameters = new
            {
                id = asset.Id,
                name = asset.Name,
                typeName = asset.TypeName,
                currencyName = asset.CurrencyName,
                balance = asset.Balance,
                periodId = asset.PeriodId
            };

            DbHelper.UpdateEntity<Asset>(queryText, queryParameters);
        }

        public void RemoveAsset(Guid assetId, Guid periodId)
        {
            string queryText = DbHelper.GetQueryText(SCRIPTS_DIR_NAME, "DeleteAsset.sql");

            object queryParameters = new
            {
                id = assetId
            };

            DbHelper.RemoveEntity<Asset>(queryText, queryParameters);
        }
    }
}
