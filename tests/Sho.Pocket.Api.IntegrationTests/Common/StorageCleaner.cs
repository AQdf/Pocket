using Dapper;
using Sho.Pocket.Domain.Entities;
using System.Data;
using System.Data.SqlClient;

namespace Sho.Pocket.Api.IntegrationTests.Common
{
    public static class StorageCleaner
    {
        public static void Cleanup()
        {
            CleanupTable(nameof(Balance));
            CleanupTable(nameof(ExchangeRate));
            CleanupTable(nameof(Asset));
            CleanupTable("Currency");
        }

        public static void CleanupTable(string tableName)
        {
            string query = $"delete from {tableName}";

            using (IDbConnection db = new SqlConnection(ConfigurationConstants.DB_CONNECTION))
            {
                db.ExecuteScalar(query);
            }
        }
    }
}
