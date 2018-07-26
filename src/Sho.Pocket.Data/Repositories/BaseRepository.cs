using Dapper;
using Sho.Pocket.Core.Configuration;
using Sho.Pocket.Core.Entities;
using Sho.Pocket.Core.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Sho.Pocket.Data.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T: BaseEntity
    {
        private readonly IDbConfiguration _dbConfiguration;

        public BaseRepository(IDbConfiguration dbConfiguration)
        {
            _dbConfiguration = dbConfiguration;
        }

        public List<T> GetAll(string queryText, object queryParameters = null)
        {
            List<T> result;

            using (IDbConnection db = new SqlConnection(_dbConfiguration.DbConnectionString))
            {
                result = db.Query<T>(queryText).ToList();
            }

            return result;
        }

        public T GetEntity(string queryText, object queryParameters = null)
        {
            T result;

            using (IDbConnection db = new SqlConnection(_dbConfiguration.DbConnectionString))
            {
                result = db.QueryFirst<T>(queryText);
            }

            return result;
        }

        public T InsertEntity(string queryText, object queryParameters = null)
        {
            T result;

            using (IDbConnection db = new SqlConnection(_dbConfiguration.DbConnectionString))
            {
                result = db.QueryFirst<T>(queryText, queryParameters);
            }

            return result;
        }

        public void UpdateEntity(string queryText, object queryParameters = null)
        {
            using (IDbConnection db = new SqlConnection(_dbConfiguration.DbConnectionString))
            {
                db.ExecuteScalar<T>(queryText, queryParameters);
            }
        }

        public void RemoveEntity(string queryText, object queryParameters = null)
        {
            using (IDbConnection db = new SqlConnection(_dbConfiguration.DbConnectionString))
            {
                db.ExecuteScalar<T>(queryText, queryParameters);
            }
        }

        public void ExecuteScript(string queryText, object queryParameters = null)
        {
            using (IDbConnection db = new SqlConnection(_dbConfiguration.DbConnectionString))
            {
                db.ExecuteScalar(queryText, queryParameters);
            }
        }

        public string GetQueryText(string dirPath, string fileName)
        {
            string result;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"Sho.Pocket.Data.Scripts.{dirPath}.{fileName}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
