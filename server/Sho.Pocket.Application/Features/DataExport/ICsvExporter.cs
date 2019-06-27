using System.Collections.Generic;

namespace Sho.Pocket.Application.DataExport
{
    public interface ICsvExporter
    {
        string ExportToCsv<T>(IList<T> data);
    }
}
