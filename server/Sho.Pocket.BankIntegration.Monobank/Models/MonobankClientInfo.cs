using System.Collections.Generic;

namespace Sho.BankIntegration.Monobank.Models
{
    /// <summary>
    /// Information about bank client and client accounts.
    /// </summary>
    public class MonobankClientInfo
    {
        public MonobankClientInfo(string name, string webHookUrl, IReadOnlyCollection<MonobankAccount> accounts)
        {
            Name = name;
            WebHookUrl = webHookUrl;
            Accounts = accounts;
        }

        /// <summary>
        /// Bank client name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// URL to get information about the new transactions.
        /// </summary>
        public string WebHookUrl { get; set; }

        /// <summary>
        /// Collection of accounts.
        /// </summary>
        public IReadOnlyCollection<MonobankAccount> Accounts { get; set; }
    }
}
