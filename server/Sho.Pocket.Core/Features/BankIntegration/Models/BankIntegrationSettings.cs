using System.Collections.Generic;

namespace Sho.Pocket.Core.Features.BankIntegration.Models
{
    public class BankIntegrationSettings
    {
        public IEnumerable<BankSettings> Banks { get; set; }
    }
}
