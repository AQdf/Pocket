using System;
using System.Linq;
using System.Collections.Generic;
using Sho.Pocket.Core.BankIntegration.Abstractions;

namespace Sho.Pocket.Application.Features.BankSync
{
    public class BankAccountServiceResolver : IBankAccountServiceResolver
    {
        private readonly IEnumerable<IBankAccountService> _bankAccountServices;

        public BankAccountServiceResolver(IEnumerable<IBankAccountService> bankAccountServices)
        {
            _bankAccountServices = bankAccountServices;
        }

        public IBankAccountService Resolve(string bankName)
        {
            IBankAccountService service = _bankAccountServices.FirstOrDefault(s => s.BankName.Equals(bankName, StringComparison.OrdinalIgnoreCase));

            if (service == null)
            {
                throw new Exception("Bank Account Service is not supported");
            }

            return service;
        }
    }
}
