using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Sho.Pocket.Data
{
    internal static class DbHelper
    {
        public static List<T> GetAll<T>(string queryText, object queryParameters = null)
        {
            List<T> result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.GetConnectionString()))
            {
                result = db.Query<T>(queryText).ToList();
            }

            return result;
        }

        public static T InsertEntity<T>(string queryText, object queryParameters = null)
        {
            T result;

            using (IDbConnection db = new SqlConnection(DbConfiguration.GetConnectionString()))
            {
                result = db.QueryFirst<T>(queryText, queryParameters);
            }

            return result;
        }

        public static void UpdateEntity<T>(string queryText, object queryParameters = null)
        {
            using (IDbConnection db = new SqlConnection(DbConfiguration.GetConnectionString()))
            {
                db.ExecuteScalar<T>(queryText, queryParameters);
            }
        }

        public static void RemoveEntity<T>(string queryText, object queryParameters = null)
        {
            using (IDbConnection db = new SqlConnection(DbConfiguration.GetConnectionString()))
            {
                db.ExecuteScalar<T>(queryText, queryParameters);
            }
        }

        public static void ExecuteScript(string queryText, object queryParameters = null)
        {
            using (IDbConnection db = new SqlConnection(DbConfiguration.GetConnectionString()))
            {
                db.ExecuteScalar(queryText, queryParameters);
            }
        }

        public static string GetQueryText(string dirPath, string fileName)
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
