using System;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.Features.Balances.Abstractions
{
    public interface IBalanceExportService
    {
        Task<byte[]> ExportUserBalancesToCsvAsync(Guid userOpenId);

        Task<byte[]> ExportJsonAsync(Guid userOpenId, DateTime effectiveDate);
    }
}
