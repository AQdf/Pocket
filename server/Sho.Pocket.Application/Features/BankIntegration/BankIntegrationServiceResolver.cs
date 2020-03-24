using System;
using System.Collections.Generic;
using System.Linq;
using Sho.Pocket.Core.Features.BankIntegration;

namespace Sho.Pocket.Application.Features.BankIntegration
{
    public class BankIntegrationServiceResolver : IBankIntegrationServiceResolver
    {
        private readonly IEnumerable<IBankIntegrationService> _bankIntegrationServices;

        public BankIntegrationServiceResolver(IEnumerable<IBankIntegrationService> bankIntegrationServices)
        {
            _bankIntegrationServices = bankIntegrationServices;
        }

        public IBankIntegrationService Resolve(string bankName)
        {
            IBankIntegrationService service = _bankIntegrationServices
                .FirstOrDefault(s => s.BankName.Equals(bankName, StringComparison.OrdinalIgnoreCase));

            if (service == null)
            {
                throw new Exception("Bank Integration Service is not supported");
            }

            return service;
        }
    }
}
