using Dapper;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sho.Pocket.DataAccess.Sql.Assets
{
    public class AssetRepository : BaseRepository<Asset>, IAssetRepository
    {
        private const string SCRIPTS_DIR_NAME = "Assets.Scripts";

        public AssetRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public List<Asset> GetAll(bool includeRelated = true)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "GetAllAssets.sql");

            List<Asset> result = includeRelated
                ? GetAllWithRelatedEntities(queryText)
                : base.GetAll(queryText);

            return result;
        }

        public Asset Add(Asset asset)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "InsertAsset.sql");

            object queryParameters = new
            {
                name = asset.Name,
                typeId = asset.TypeId,
                currencyId = asset.CurrencyId
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
                typeId = asset.TypeId,
                currencyId = asset.CurrencyId
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

        private List<Asset> GetAllWithRelatedEntities(string queryText)
        {
            List<Asset> result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = db.Query<Asset, AssetType, Currency, Asset>(queryText,
                    (asset, type, currency) =>
                    {
                        asset.Type = type;
                        asset.Currency = currency;

                        return asset;
                    }).ToList();
            }

            return result;
        }
    }
}
