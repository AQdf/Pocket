using System.Collections.Generic;
using Sho.Pocket.Application.Balances.Models;

namespace Sho.Pocket.Application.Balances
{
    public class BalanceExporter : IBalanceExporter
    {
        public string ExportToCsv(List<BalanceExportModel> balances)
        {
            CsvWriter writer = new CsvWriter();
            string result = writer.Write<BalanceExportModel>(balances);

            return result;
        }
    }
}
