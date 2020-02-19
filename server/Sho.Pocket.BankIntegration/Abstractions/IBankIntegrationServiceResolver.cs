namespace Sho.Pocket.BankIntegration.Abstractions
{
    public interface IBankIntegrationServiceResolver
    {
        IBankIntegrationService Resolve(string bankName);
    }
}
