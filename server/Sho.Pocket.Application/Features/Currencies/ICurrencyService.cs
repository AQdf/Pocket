using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Application.Currencies
{
    public interface ICurrencyService
    {
        Task<List<string>> GetCurrencies();
    }
}
