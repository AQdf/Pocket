using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System;
using System.Collections.Generic;

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

            List<Asset> result = base.GetAll(queryText);

            return result;
        }

        public Asset Add(string name, Guid currencyId, bool isActive)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "InsertAsset.sql");

            object queryParameters = new { name, currencyId, isActive };

            Asset result = base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public void Update(Guid id, string name, bool isActive)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "UpdateAsset.sql");

            object queryParameters = new { id, name, isActive };

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

        public bool ExistsAssetBalance(Guid id)
        {
            string queryText = @"
                if exists (select * from Balance)
                select 1
                else select 0";

            bool result = base.Exists(queryText);

            return result;
        }
    }
}
