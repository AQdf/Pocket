using System.Threading.Tasks;
using Sho.Pocket.Api.IntegrationTests.Contexts;
using TechTalk.SpecFlow;

namespace Sho.Pocket.Api.IntegrationTests.Currencies.Steps
{
    [Binding]
    public class CreateCurrencyStep
    {
        private readonly CurrencyFeatureContext _context;

        public CreateCurrencyStep(CurrencyFeatureContext currencyFeatureContext)
        {
            _context = currencyFeatureContext;
        }

        [Given(@"currency (.*) exists")]
        public async Task GivenCurrency(string currencyName)
        {
            bool exists = await _context.CurrencyRepository.ExistsAsync(currencyName);

            if (!exists)
            {
                await _context.CurrencyRepository.CreateAsync(currencyName);
            }
        }
    }
}
