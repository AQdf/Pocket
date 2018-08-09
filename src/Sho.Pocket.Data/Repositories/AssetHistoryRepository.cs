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
                assetId = assetHistory.AssetId,
                effectiveDate = assetHistory.EffectiveDate,
                exchangeRateId = assetHistory.ExchangeRateId,
                balance = assetHistory.Balance
            };

            AssetHistory result = base.InsertEntity(queryText, queryParameters);

            return result;
        }

        public List<AssetHistory> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid assetId)
        {
            throw new NotImplementedException();
        }

        public void Update(AssetHistory asset)
        {
            throw new NotImplementedException();
        }
    }
}
