namespace Sho.Pocket.Core.Features.BankSync.Abstractions
{
    public interface IBankAccountServiceResolver
    {
        IBankAccountService Resolve(string bankName);
    }
}
