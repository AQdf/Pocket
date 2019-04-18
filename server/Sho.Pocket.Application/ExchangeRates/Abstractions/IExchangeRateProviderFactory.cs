namespace Sho.Pocket.Application.ExchangeRates.Abstractions
{
    public interface IExchangeRateProviderFactory
    {
        IExchangeRateProvider GetProvider(string name);
    }
}
