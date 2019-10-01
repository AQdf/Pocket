using Sho.Pocket.Application.Features.Balances.Models;
using System;
using System.Threading.Tasks;

namespace Sho.Pocket.Application.Features.Balances
{
    public interface IBalanceNoteService
    {
        Task<BalanceNoteViewModel> GetNoteByIdAsync(Guid userOpenId, Guid id);

        Task<BalanceNoteViewModel> GetNoteByEffectiveDateAsync(Guid userOpenId, DateTime effectiveDate);

        Task<BalanceNoteViewModel> AlterNoteAsync(Guid userOpenId, DateTime effectiveDate, string content);

        Task<BalanceNoteViewModel> AddNoteAsync(Guid userOpenId, DateTime effectiveDate, string content);

        Task<BalanceNoteViewModel> UpdateNoteAsync(Guid userOpenId, Guid id, string content);
    }
}
