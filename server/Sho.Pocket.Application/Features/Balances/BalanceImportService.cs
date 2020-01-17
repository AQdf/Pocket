using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.Balances.Abstractions;
using Sho.Pocket.Core.Features.Balances.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Features.Balances
{
    public class BalanceImportService : IBalanceImportService
    {
        private readonly IBalanceRepository _balanceRepository;

        private readonly IAssetRepository _assetRepository;

        private readonly IExchangeRateRepository _exchangeRateRepository;

        public BalanceImportService(
            IBalanceRepository balanceRepository,
            IAssetRepository assetRepository,
            IExchangeRateRepository exchangeRateRepository)
        {
            _balanceRepository = balanceRepository;
            _assetRepository = assetRepository;
            _exchangeRateRepository = exchangeRateRepository;
        }

        public async Task ImportJsonAsync(Guid userOpenId, string jsonData)
        {
            List<BalanceExportModel> items = JsonConvert.DeserializeObject<List<BalanceExportModel>>(jsonData);
            IEnumerable<IGrouping<DateTime, BalanceExportModel>> efeectiveDateGroup = items.GroupBy(i => i.EffectiveDate);

            foreach (IGrouping<DateTime, BalanceExportModel> group in efeectiveDateGroup)
            {
                bool exists = await _balanceRepository.ExistsEffectiveDateBalancesAsync(userOpenId, group.Key);

                if (!exists)
                {
                    foreach (BalanceExportModel item in group)
                    {
                        Asset asset = await _assetRepository.GetByNameAsync(userOpenId, item.Asset);

                        if (asset != null)
                        {
                            ExchangeRate rate = await _exchangeRateRepository.AlterAsync(item.EffectiveDate, item.Currency, item.CounterCurrency, item.ExchangeRate);
                            Balance balance = await _balanceRepository.CreateAsync(userOpenId, asset.Id, item.EffectiveDate, item.Value, rate.Id);
                        }
                    }
                }
            }
        }
    }
}
