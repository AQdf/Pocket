using System.Collections.Generic;

namespace Sho.BankIntegration.Monobank.Models
{
    /// <summary>
    /// Example: https://api.monobank.ua/docs/#/definitions/UserInfo
    /// </summary>
    internal class MonobankClientInfo
    {
        /// <summary>
        /// Client name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// URL to get information about the new transaction
        /// </summary>
        public string WebHookUrl { get; set; }

        /// <summary>
        /// Collection of accounts
        /// </summary>
        public IReadOnlyCollection<MonobankClientAccount> Accounts { get; set; }
    }
}