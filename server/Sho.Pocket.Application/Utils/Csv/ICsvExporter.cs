using System.Collections.Generic;

namespace Sho.Pocket.Application.Utils.Csv
{
    public interface ICsvExporter
    {
        string ExportToCsv<T>(IList<T> data);
    }
}
