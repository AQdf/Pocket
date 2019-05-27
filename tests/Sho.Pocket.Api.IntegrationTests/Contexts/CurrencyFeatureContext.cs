using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sho.Pocket.Api.IntegrationTests.Contexts
{
    public class CurrencyFeatureContext : FeatureContextBase
    {
        public List<string> Currencies { get; set; }

        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyFeatureContext() : base()
        {
            Currencies = new List<string>();

            _currencyRepository = _serviceProvider.GetRequiredService<ICurrencyRepository>();
        }

        public async Task<string> AddCurrency(string currencyName)
        {
            var exists = await _currencyRepository.ExistsAsync(currencyName);

            if (!exists)
            {
                // TODO: Fix issue with duplicate insert because of parallel execution
                try
                {
                    Currency currency = await _currencyRepository.CreateAsync(currencyName);
                    Currencies.Add(currency.Name);
                }
                catch (System.Exception)
                {
                    return currencyName;
                }
            }

            return currencyName;
        }
    }
}
