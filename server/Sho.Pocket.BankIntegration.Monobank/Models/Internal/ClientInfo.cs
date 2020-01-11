using System.Collections.Generic;

namespace Sho.BankIntegration.Monobank.Models.Internal
{
    /// <summary>
    /// Example: https://api.monobank.ua/docs/#/definitions/UserInfo
    /// </summary>
    internal class ClientInfo
    {
        /// <summary>
        /// Client name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// URL to get information about the new transactions.
        /// </summary>
        public string WebHookUrl { get; set; }

        /// <summary>
        /// Collection of accounts.
        /// </summary>
        public IReadOnlyCollection<ClientAccount> Accounts { get; set; }
    }
}