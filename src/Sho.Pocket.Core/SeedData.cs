using Sho.Pocket.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sho.Pocket.Core
{
    internal static class SeedData
    {
        public static List<PeriodSummary> GetDefaultPeriods()
        {
            return new List<PeriodSummary>
            {
                GetDefaultPeriodSummary(DateTime.Now, GetAsset1()),
                GetDefaultPeriodSummary(DateTime.Now.AddMonths(-1), GetAsset2())
            };
        }

        public static PeriodSummary GetDefaultPeriodSummary(DateTime reportedDate, List<Asset> assets)
        {
            return new PeriodSummary
            {
                Id = Guid.NewGuid(),
                Assets = assets,
                ReportedDate = reportedDate,
                TotalBalanceUAH = assets.Sum(a => a.Balance),
                //Currency = assets.FirstOrDefault()?.Balance.Currency
            };
        }
        
        public static List<Asset> GetAsset1()
        {
            return new List<Asset>
            {
                new Asset
                {
                    Id = Guid.NewGuid(),
                    Name = "PrivatBank Deposit",
                    Balance = 1000M
                },
                new Asset
                {
                    Id = Guid.NewGuid(),
                    Name = "UkrSibBank account",
                    Balance = 2000M
                }
            };
        }

        public static List<Asset> GetAsset2()
        {
            return new List<Asset>
            {
                new Asset
                {
                    Id = Guid.NewGuid(),
                    Name = "USD Cash",
                    Balance = 3000M
                },
                new Asset
                {
                    Id = Guid.NewGuid(),
                    Name = "EUR Cash",
                    Balance = 4000M
                }
            };
        }
    }
}
