using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;

namespace Sho.Pocket.Application.Currencies
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<List<string>> GetCurrencies()
        {
            IEnumerable<string> currencies = await _currencyRepository.GetAllAsync();

            return currencies.ToList();
        }
    }
}
