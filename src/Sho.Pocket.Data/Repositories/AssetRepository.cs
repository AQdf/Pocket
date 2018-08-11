using Sho.Pocket.Core.Configuration;
using Sho.Pocket.Core.Entities;
using Sho.Pocket.Core.Repositories;
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

        public List<Asset> GetAll()
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "GetAllAssets.sql");

            List<Asset> result = base.GetAll(queryText);

            return result;
        }

        public Asset Add(Asset asset)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "InsertAsset.sql");

            object queryParameters = new
            {
                name = asset.Name,
                typeName = asset.TypeName,
                currencyName = asset.CurrencyName,
                balance = asset.Balance
            };

            Asset result = base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public void Update(Asset asset)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "UpdateAsset.sql");

            object queryParameters = new
            {
                id = asset.Id,
                name = asset.Name,
                typeName = asset.TypeName,
                currencyName = asset.CurrencyName,
                balance = asset.Balance
            };

            base.UpdateEntity(queryText, queryParameters);
        }

        public void Remove(Guid assetId)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "DeleteAsset.sql");

            object queryParameters = new
            {
                id = assetId
            };

            base.RemoveEntity(queryText, queryParameters);
        }

        public void DeactivateAsset(Guid assetId)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "DeactivateAsset.sql");

            object queryParameters = new
            {
                id = assetId
            };

            base.ExecuteScript(queryText, queryParameters);
        }
    }
}
