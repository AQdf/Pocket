using System;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.Features.Balances.Abstractions
{
    public interface IBalanceImportService
    {
        Task ImportJsonAsync(Guid userId, string jsonData);
    }
}
