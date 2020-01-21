using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sho.Pocket.Application.Utils.Csv;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.Balances.Abstractions;
using Sho.Pocket.Core.Features.Balances.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Features.Balances
{
    public class BalanceExportService : IBalanceExportService
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly ICsvExporter _csvExporter;

        public BalanceExportService(IBalanceRepository balanceRepository, ICsvExporter balanceExporter)
        {
            _balanceRepository = balanceRepository;
            _csvExporter = balanceExporter;
        }

        public async Task<byte[]> ExportUserBalancesToCsvAsync(Guid userOpenId)
        {
            IEnumerable<Balance> balances = await _balanceRepository.GetAllAsync(userOpenId);

            List<BalanceExportModel> items = balances
                .Select(b => new BalanceExportModel(b.EffectiveDate, b.Asset, b.ExchangeRate, b.Value))
                .ToList();

            string csv = _csvExporter.ExportToCsv(items);
            byte[] bytes = Encoding.Default.GetBytes(csv);

            return bytes;
        }

        public async Task<byte[]> ExportJsonAsync(Guid userOpenId, DateTime effectiveDate)
        {
            IEnumerable<Balance> balances = await _balanceRepository.GetByEffectiveDateAsync(userOpenId, effectiveDate);

            List<BalanceExportModel> items = balances
                .Select(b => new BalanceExportModel(b.EffectiveDate, b.Asset, b.ExchangeRate, b.Value))
                .ToList();

            string csv = JsonConvert.SerializeObject(items);
            byte[] bytes = Encoding.Default.GetBytes(csv);

            return bytes;
        }
    }
}
