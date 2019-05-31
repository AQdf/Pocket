using Dapper;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Sho.Pocket.DataAccess.Sql
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        public readonly IDbConfiguration DbConfiguration;

        public BaseRepository(IDbConfiguration dbConfiguration)
        {
            DbConfiguration = dbConfiguration;
        }

        public async Task<IEnumerable<T>> GetEntities(string queryText, object queryParameters = null)
        {
            IEnumerable<T> result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = await db.QueryAsync<T>(queryText, queryParameters);
            }

            return result;
        }

        public async Task<T> GetEntity(string queryText, object queryParameters = null)
        {
            T result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = await db.QueryFirstOrDefaultAsync<T>(queryText, queryParameters);
            }

            return result;
        }

        public async Task<T> InsertEntity(string queryText, object queryParameters = null)
        {
            T result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = await db.QueryFirstOrDefaultAsync<T>(queryText, queryParameters);
            }

            return result;
        }

        public async Task<T> UpdateEntity(string queryText, object queryParameters = null)
        {
            T result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = await db.QueryFirstAsync<T>(queryText, queryParameters);
            }

            return result;
        }

        public async Task DeleteEntity(string queryText, object queryParameters = null)
        {
            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                await db.ExecuteScalarAsync(queryText, queryParameters);
            }
        }

        public async Task<bool> Exists(string queryText, object queryParameters = null)
        {
            bool result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = await db.ExecuteScalarAsync<bool>(queryText, queryParameters);
            }

            return result;
        }

        public async Task ExecuteScript(string queryText, object queryParameters = null)
        {
            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                await db.ExecuteScalarAsync(queryText, queryParameters);
            }
        }

        public async Task<string> GetQueryText(string dirPath, string fileName)
        {
            string result;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"Sho.Pocket.DataAccess.Sql.{dirPath}.{fileName}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = await reader.ReadToEndAsync();
            }

            return result;
        }
    }
}
