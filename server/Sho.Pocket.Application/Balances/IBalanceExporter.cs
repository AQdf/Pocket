using Sho.Pocket.Application.Balances.Models;
using System.Collections.Generic;

namespace Sho.Pocket.Application.Balances
{
    public interface IBalanceExporter
    {
        string ExportToCsv(List<BalanceExportModel> balances);
    }
}
