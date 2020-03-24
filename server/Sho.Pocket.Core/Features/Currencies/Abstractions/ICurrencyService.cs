using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.Features.Currencies.Abstractions
{
    public interface ICurrencyService
    {
        Task<List<string>> GetCurrenciesAsync();

        Task<string> AddAsync(string name);

        Task<bool> ExistsAsync(string name);
    }
}
