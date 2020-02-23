using System;
using System.Threading.Tasks;
using Sho.Pocket.Core.Features.Balances.Models;

namespace Sho.Pocket.Core.Features.Balances.Abstractions
{
    public interface IBalanceNoteService
    {
        Task<BalanceNoteViewModel> GetNoteByIdAsync(Guid userOpenId, Guid id);

        Task<BalanceNoteViewModel> GetNoteByEffectiveDateAsync(Guid userOpenId, DateTime effectiveDate);

        Task<BalanceNoteViewModel> AlterNoteAsync(Guid userOpenId, DateTime effectiveDate, string content);
    }
}
