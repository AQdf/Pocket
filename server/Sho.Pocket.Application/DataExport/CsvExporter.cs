using Sho.Pocket.Application.DataExport;
using System.Collections.Generic;

namespace Sho.Pocket.Application.Balances
{
    public class CsvExporter : ICsvExporter
    {
        public string ExportToCsv<T>(IList<T> data)
        {
            CsvWriter writer = new CsvWriter();
            string result = writer.Write<T>(data);

            return result;
        }
    }
}
