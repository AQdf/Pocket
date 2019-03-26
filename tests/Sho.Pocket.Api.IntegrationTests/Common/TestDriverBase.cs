using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Application.Common.Configuration;
using Sho.Pocket.Core;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sho.Pocket.Api.IntegrationTests.Common
{
    public abstract class TestDriverBase
    {
        protected ServiceProvider _serviceProvider;

        private string _dbConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=PocketDb-Test;Integrated Security=true;";

        public TestDriverBase()
        {
            Configure();
        }

        private void Configure()
        {
            IServiceCollection services = new ServiceCollection();

            GlobalSettings globalSettings = new GlobalSettings
            {
                DbConnectionString = _dbConnectionString
            };

            services.AddSingleton(s => globalSettings);

            services.AddApplicationServices();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected void ExecuteScript(string queryText, object queryParameters = null)
        {
            using (IDbConnection db = new SqlConnection(_dbConnectionString))
            {
                var result = db.ExecuteScalar(queryText, queryParameters);
            }
        }

        protected List<T> GetList<T>(string queryText, object queryParameters = null)
        {
            List<T> result;

            using (IDbConnection db = new SqlConnection(_dbConnectionString))
            {
                result = db.Query<T>(queryText).ToList();
            }

            return result;
        }
    }
}
