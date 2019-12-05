using System.Collections.Generic;

namespace Sho.Pocket.BankIntegration.Monobank.Models
{
    /// <summary>
    /// Monobank API ref: https://api.monobank.ua/docs/#/definitions/UserInfo
    /// </summary>
    internal class MonobankUserInfo
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
        public IReadOnlyCollection<MonobankAccount> Accounts { get; set; }
    }
}