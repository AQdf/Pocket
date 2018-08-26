using Sho.Pocket.Core;
using Sho.Pocket.Core.DataAccess;

namespace Sho.Pocket.DataAccess.Sql
{
    public class DbConfiguration : IDbConfiguration
    {
        public string DbConnectionString { get; }

        public DbConfiguration(GlobalSettings globalSettings)
        {
            DbConnectionString = globalSettings.DbConnectionString;
        }
    }
}
