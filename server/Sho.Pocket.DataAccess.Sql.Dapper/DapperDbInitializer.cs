using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Options;
using Sho.Pocket.Core.DataAccess.Configuration;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.Dapper
{
    public class DapperDbInitializer : IDbInitializer
    {
        private readonly string _connectionString;

        private readonly List<Currency> _defaultCurrencies;

        public DapperDbInitializer(IOptionsMonitor<DbSettings> options)
        {
            _connectionString = options.CurrentValue.DbConnectionString;

            _defaultCurrencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("UAH"),
                new Currency("EUR"),
                new Currency("PLN")
            };

            if (!string.IsNullOrWhiteSpace(options.CurrentValue.SystemDefaultCurrency) 
                && !_defaultCurrencies.Exists(c => c.Name == options.CurrentValue.SystemDefaultCurrency))
            {
                _defaultCurrencies.Add(new Currency(options.CurrentValue.SystemDefaultCurrency));
            }
        }

        public void EnsureCreated()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                System.Console.WriteLine($"Dapper SQL connection state: db.State");
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

            using (IDbConnection db = new SqlConnection(_connectionString))
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

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Execute(queryText);
            }
        }
    }
}
