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
    public abstract class FeatureManagerBase
    {
        protected ServiceProvider _serviceProvider;

        public FeatureManagerBase()
        {
            Configure();
        }

        private void Configure()
        {
            IServiceCollection services = new ServiceCollection();

            GlobalSettings globalSettings = new GlobalSettings
            {
                DbConnectionString = ConfigurationConstants.DB_CONNECTION
            };

            services.AddSingleton(s => globalSettings);

            services.AddApplicationServices();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected void ExecuteScript(string queryText, object queryParameters = null)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationConstants.DB_CONNECTION))
            {
                db.ExecuteScalar(queryText, queryParameters);
            }
        }

        protected List<T> GetList<T>(string queryText, object queryParameters = null)
        {
            List<T> result;

            using (IDbConnection db = new SqlConnection(ConfigurationConstants.DB_CONNECTION))
            {
                result = db.Query<T>(queryText).ToList();
            }

            return result;
        }
    }
}
