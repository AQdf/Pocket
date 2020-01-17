using System.Collections.Generic;

namespace Sho.Pocket.Application.Utils.Csv
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
