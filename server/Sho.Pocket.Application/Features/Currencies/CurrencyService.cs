using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.Currencies.Abstractions;
using Sho.Pocket.Domain.Entities;

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
            IEnumerable<Currency> currencies = await _currencyRepository.GetAllAsync();
            List<string> result = currencies.Select(c => c.Name).ToList();

            return result;
        }
    }
}
