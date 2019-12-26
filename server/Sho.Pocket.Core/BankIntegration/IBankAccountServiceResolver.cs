namespace Sho.Pocket.Core.BankIntegration
{
    public interface IBankAccountServiceResolver
    {
        IBankAccountService Resolve(string bankName);
    }
}
