using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Api.IntegrationTests.Common;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;
using System.Collections.Generic;

namespace Sho.Pocket.Api.IntegrationTests.Currencies.Managers
{
    public class CurrencyFeatureManager : FeatureManagerBase
    {
        public Dictionary<string, Currency> Currencies { get; set; }

        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyFeatureManager() : base()
        {
            Currencies = new Dictionary<string, Currency>();

            _currencyRepository = _serviceProvider.GetRequiredService<ICurrencyRepository>();
        }

        public Currency AddCurrency(string currencyName)
        {
            if (!Currencies.ContainsKey(currencyName))
            {
                Currency currency = _currencyRepository.Add(currencyName);
                Currencies.Add(currencyName, currency);
            }

            Currency result = Currencies[currencyName];

            return result;
        }
    }
}
