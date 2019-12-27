namespace Sho.Pocket.Core.BankIntegration.Abstractions
{
    public interface IBankAccountServiceResolver
    {
        IBankAccountService Resolve(string bankName);
    }
}
