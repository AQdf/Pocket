using System;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.Balances.Models;

namespace Sho.Pocket.Core.Features.Balances.Abstractions
{
    public interface IBalanceNoteService
    {
        Task<BalanceNoteViewModel> GetNoteByIdAsync(Guid userId, Guid id);

        Task<BalanceNoteViewModel> GetNoteByEffectiveDateAsync(Guid userId, DateTime effectiveDate);

        Task<BalanceNoteViewModel> AlterNoteAsync(Guid userId, DateTime effectiveDate, string content);
    }
}
