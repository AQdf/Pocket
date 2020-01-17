namespace Sho.Pocket.Core.Features.BankAccounts.Abstractions
{
    public interface IBankAccountServiceResolver
    {
        IBankAccountService Resolve(string bankName);
    }
}
