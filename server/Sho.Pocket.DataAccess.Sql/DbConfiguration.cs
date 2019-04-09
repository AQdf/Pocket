using Dapper;
using Sho.Pocket.Core;
using Sho.Pocket.Core.DataAccess;
using System.Data;
using System.Data.SqlClient;

namespace Sho.Pocket.DataAccess.Sql
{
    public class DbConfiguration : IDbConfiguration
    {
        public string DbConnectionString { get; }

        public DbConfiguration(GlobalSettings globalSettings)
        {
            DbConnectionString = globalSettings.DbConnectionString;
        }

        public void SeedStorageData()
        {
            string queryText = @"
                IF NOT EXISTS (select top 1 1 from [dbo].[Currency])
                BEGIN
                    INSERT INTO [dbo].[Currency] ([Id], [Name], [Description], [IsDefault])
                    VALUES 
                    (NEWID(), 'UAH', 'Ukrainian Hryvnya', 1),
                    (NEWID(), 'USD', 'American Dollar', 0),
                    (NEWID(), 'EUR', 'Euro', 0),
                    (NEWID(), 'PLN', 'Polish zloty', 0)
                END";

            using (IDbConnection db = new SqlConnection(DbConnectionString))
            {
                db.ExecuteScalar(queryText);
            }
        }
    }
}
