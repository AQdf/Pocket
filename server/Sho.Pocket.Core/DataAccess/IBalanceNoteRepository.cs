using Sho.Pocket.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IBalanceNoteRepository
    {
        Task<BalanceNote> GetByIdAsync(Guid userOpenId, Guid id);

        Task<BalanceNote> GetByEffectiveDateAsync(Guid userOpenId, DateTime effectiveDate);

        Task<BalanceNote> AlterAsync(Guid userOpenId, DateTime effectiveDate, string content);

        Task<BalanceNote> CreateAsync(Guid userOpenId, DateTime effectiveDate, string content);

        Task<BalanceNote> UpdateAsync(Guid userOpenId, Guid id, string content);
    }
}
