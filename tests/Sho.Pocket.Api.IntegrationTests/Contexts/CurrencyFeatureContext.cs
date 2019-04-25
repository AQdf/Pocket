using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Api.IntegrationTests.Contexts
{
    public class CurrencyFeatureContext : FeatureContextBase
    {
        public Dictionary<string, Currency> Currencies { get; set; }

        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyFeatureContext() : base()
        {
            Currencies = new Dictionary<string, Currency>();

            _currencyRepository = _serviceProvider.GetRequiredService<ICurrencyRepository>();
        }

        public async Task<Currency> AddCurrency(string currencyName)
        {
            if (!Currencies.ContainsKey(currencyName))
            {
                Currency currency = await _currencyRepository.Add(currencyName);
                Currencies.Add(currencyName, currency);
            }

            Currency result = Currencies[currencyName];

            return result;
        }
    }
}
