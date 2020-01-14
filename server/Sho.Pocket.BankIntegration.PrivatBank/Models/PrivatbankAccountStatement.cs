using System.Collections.Generic;

namespace Sho.BankIntegration.Privatbank.Models
{
    /// <summary>
    /// Example: <https://api.privatbank.ua/#p24/orders>.
    /// </summary>
    public class PrivatbankAccountStatement
    {
        public PrivatbankAccountStatement(string status, decimal? credit, decimal? debet, IReadOnlyCollection<PrivatbankAccountTransaction> items)
        {
            Status = status;
            Credit = credit;
            Debet = debet;
            Items = items;
        }

        /// <summary>
        /// Possible values: [excellent].
        /// </summary>
        public string Status { get; }

        /// <summary>
        /// Total amount of expenses.
        /// </summary>
        public decimal? Credit { get; }

        /// <summary>
        /// Total amount of income.
        /// </summary>
        public decimal? Debet { get; }

        /// <summary>
        /// Collection of account transactions.
        /// </summary>
        public IReadOnlyCollection<PrivatbankAccountTransaction> Items { get; }
    }
}
