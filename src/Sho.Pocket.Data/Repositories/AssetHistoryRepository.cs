using System;
using System.Collections.Generic;
using Sho.Pocket.Core.Configuration;
using Sho.Pocket.Core.Entities;
using Sho.Pocket.Core.Repositories;

namespace Sho.Pocket.Data.Repositories
{
    public class AssetHistoryRepository : BaseRepository<AssetHistory>, IAssetHistoryRepository
    {
        private const string SCRIPTS_DIR_NAME = "AssetHistory";

        public AssetHistoryRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public AssetHistory Add(AssetHistory assetHistory)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "InsertAssetHistory.sql");

            object queryParameters = new
            {
                assetName = assetHistory.AssetName,
                effectiveDate = assetHistory.EffectiveDate,
                exchangeRateId = assetHistory.ExchangeRateId,
                balance = assetHistory.Balance
            };

            AssetHistory result = base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public List<AssetHistory> GetAll()
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "GetAllAssetHistory.sql");

            List<AssetHistory> result = base.GetAll(queryText);

            return result;
        }

        public void Remove(Guid assetHistoryId)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "DeleteAssetHistory.sql");

            object queryParameters = new
            {
                id = assetHistoryId
            };

            base.RemoveEntity(queryText, queryParameters);
        }

        public void Update(AssetHistory assetHistory)
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "UpdateAssetHistory.sql");

            object queryParameters = new
            {
                id = assetHistory.Id,
                assetName = assetHistory.AssetName,
                effectiveDate = assetHistory.EffectiveDate,
                exchangeRateId = assetHistory.ExchangeRateId,
                balance = assetHistory.Balance
            };

            base.UpdateEntity(queryText, queryParameters);
        }
    }
}
