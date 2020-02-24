using System;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.Features.Balances.Abstractions
{
    public interface IBalanceExportService
    {
        Task<byte[]> ExportUserBalancesToCsvAsync(Guid userId);

        Task<byte[]> ExportJsonAsync(Guid userId, DateTime effectiveDate);
    }
}
