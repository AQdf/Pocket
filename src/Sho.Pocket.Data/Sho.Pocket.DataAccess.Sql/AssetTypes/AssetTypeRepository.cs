using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;

namespace Sho.Pocket.DataAccess.Sql.AssetTypes
{
    public class AssetTypeRepository : BaseRepository<AssetType>, IAssetTypeRepository
    {
        private const string SCRIPTS_DIR_NAME = "AssetTypes.Scripts";

        public AssetTypeRepository(IDbConfiguration dbConfiguration) : base(dbConfiguration)
        {
        }

        public List<AssetType> GetAll()
        {
            string queryText = GetQueryText(SCRIPTS_DIR_NAME, "GetAllAssetTypes.sql");

            return base.GetAll(queryText);
        }
    }
}
