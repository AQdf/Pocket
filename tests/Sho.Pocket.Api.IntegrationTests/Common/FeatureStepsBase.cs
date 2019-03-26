using Dapper;
using System.Data;
using System.Data.SqlClient;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Common
{
    internal abstract class FeatureStepsBase
    {
        [BeforeScenario]
        [AfterScenario]
        protected virtual void CleanupStorage()
        {
            const string query = @"
                delete from Balance;
                delete from ExchangeRate;
                delete from Asset;
                delete from Currency;";

            using (IDbConnection db = new SqlConnection(ConfigurationConstants.DB_CONNECTION))
            {
                db.ExecuteScalar(query);
            }
        }
    }
}
