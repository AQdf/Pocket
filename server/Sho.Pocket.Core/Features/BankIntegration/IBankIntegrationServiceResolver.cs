namespace Sho.Pocket.Core.Features.BankIntegration
{
    public interface IBankIntegrationServiceResolver
    {
        IBankIntegrationService Resolve(string bankName);
    }
}
