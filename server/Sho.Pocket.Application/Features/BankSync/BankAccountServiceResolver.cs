using System;
using Microsoft.Extensions.DependencyInjection;
using Sho.Pocket.Core.BankIntegration;
using Sho.Pocket.BankIntegration.Monobank.Abstractions;
using Sho.Pocket.BankIntegration.Privatbank.Abstractions;

namespace Sho.Pocket.Application.Features.BankSync
{
    public class BankAccountServiceResolver : IBankAccountServiceResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public BankAccountServiceResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IBankAccountService Resolve(string bankName)
        {
            switch (bankName)
            {
                case BankNameConstants.MONOBANK:
                {
                    return _serviceProvider.GetService<IMonobankAccountService>();
                }
                case BankNameConstants.PRIVATBANK:
                {
                    return _serviceProvider.GetService<IPrivatbankAccountService>();
                }
                default:
                {
                    throw new Exception("Bank is not supported");
                }
            }
        }
    }
}
