using Dapper;
using Sho.Pocket.Core.Configuration.Models;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sho.Pocket.DataAccess.Sql
{
    public class DbConfiguration : IDbConfiguration
    {
        public string DbConnectionString { get; }

        private readonly List<Currency> _defaultCurrencies;

        public DbConfiguration(GlobalSettings globalSettings)
        {
            DbConnectionString = globalSettings.DbConnectionString;

            _defaultCurrencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("UAH"),
                new Currency("EUR"),
                new Currency("PLN")
            };

            if (!string.IsNullOrWhiteSpace(globalSettings.DefaultCurrency) 
                && !_defaultCurrencies.Exists(c => c.Name == globalSettings.DefaultCurrency))
            {
                _defaultCurrencies.Add(new Currency(globalSettings.DefaultCurrency));
            }
        }

        public void SeedStorageData()
        {
            AddDefaultCurrencies();
            AddSupportedBanks();
        }

        private void AddDefaultCurrencies()
        {
            string queryText = @"
                IF NOT EXISTS (select top 1 1 from [dbo].[Currency] where [Name] = @Name)
                BEGIN
                    INSERT INTO [dbo].[Currency] ([Name]) VALUES (@Name)
                END";

            using (IDbConnection db = new SqlConnection(DbConnectionString))
            {
                db.Execute(queryText, _defaultCurrencies);
            }
        }

        private void AddSupportedBanks()
        {
            string queryText = @"
                IF NOT EXISTS (select top 1 1 from [dbo].[Bank] where [Name] = 'Monobank')
                BEGIN
                    INSERT INTO [dbo].[Bank] ([Name], [Country], [Active], [ApiUrl], [SyncFreqInSeconds]) VALUES ('Monobank', 'Ukraine', 1, NULL, NULL)
                END";

            using (IDbConnection db = new SqlConnection(DbConnectionString))
            {
                db.Execute(queryText);
            }
        }
    }
}
