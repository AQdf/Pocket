using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sho.Pocket.Application.Configuration.Models;
using Sho.Pocket.BankIntegration.Abstractions;
using Sho.Pocket.BankIntegration.Models;

namespace Sho.Pocket.Application.Features.BankIntegration
{
    public class BankService : IBankService
    {
        private readonly IEnumerable<BankSettings> _settings;

        public BankService(IOptionsMonitor<BankIntegrationSettings> options)
        {
            _settings = options.CurrentValue.Banks;
        }

        public IReadOnlyCollection<BankModel> GetBanks()
        {
            return _settings.Select(s => new BankModel(s.Name, s.ApiUri, s.SyncFreqInSeconds, s.IsActive)).ToList();
        }

        public BankModel GetBank(string bankName)
        {
            BankModel bank = GetBanks().FirstOrDefault(b => b.Name.Equals(bankName, StringComparison.OrdinalIgnoreCase));

            if (bank == null)
            {
                throw new Exception($"Bank {bankName} is not available.");
            }

            if (!bank.IsActive)
            {
                throw new Exception($"Bank {bankName} is not active.");
            }

            return bank;
        }
    }
}
