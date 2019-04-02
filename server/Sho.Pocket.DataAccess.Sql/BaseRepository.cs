using Dapper;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Sho.Pocket.DataAccess.Sql
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T: BaseEntity
    {
        public readonly IDbConfiguration DbConfiguration;

        public BaseRepository(IDbConfiguration dbConfiguration)
        {
            DbConfiguration = dbConfiguration;
        }

        public List<T> GetAll(string queryText, object queryParameters = null)
        {
            List<T> result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = db.Query<T>(queryText).ToList();
            }

            return result;
        }

        public T GetEntity(string queryText, object queryParameters = null)
        {
            T result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = db.QueryFirst<T>(queryText, queryParameters);
            }

            return result;
        }

        public T InsertEntity(string queryText, object queryParameters = null)
        {
            T result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = db.QueryFirst<T>(queryText, queryParameters);
            }

            return result;
        }

        public T UpdateEntity(string queryText, object queryParameters = null)
        {
            T result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = db.QueryFirst<T>(queryText, queryParameters);
            }

            return result;
        }

        public void RemoveEntity(string queryText, object queryParameters = null)
        {
            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                db.ExecuteScalar(queryText, queryParameters);
            }
        }

        public bool Exists(string queryText, object queryParameters = null)
        {
            bool result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                result = db.ExecuteScalar<bool>(queryText, queryParameters);
            }

            return result;
        }

        public void ExecuteScript(string queryText, object queryParameters = null)
        {
            using (IDbConnection db = new SqlConnection(DbConfiguration.DbConnectionString))
            {
                db.ExecuteScalar(queryText, queryParameters);
            }
        }

        public string GetQueryText(string dirPath, string fileName)
        {
            string result;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"Sho.Pocket.DataAccess.Dapper.{dirPath}.{fileName}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
