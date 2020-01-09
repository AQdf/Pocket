using System.Collections.Generic;

namespace Sho.Pocket.Application.Configuration.Models
{
    public class BankIntegrationSettings
    {
        public IEnumerable<BankSettings> Banks { get; set; }
    }
}
