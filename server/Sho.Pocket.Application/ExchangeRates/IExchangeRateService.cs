using Sho.Pocket.Application.ExchangeRates.Models;

namespace Sho.Pocket.Application.ExchangeRates
{
    public interface IExchangeRateService
    {
        ExchangeRateModel AddExchangeRate(ExchangeRateModel model);
    }
}
