using Sho.Pocket.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Sho.Pocket.Core.DataAccess
{
    public interface IBalanceNoteRepository
    {
        Task<BalanceNote> GetByIdAsync(Guid userId, Guid id);

        Task<BalanceNote> GetByEffectiveDateAsync(Guid userId, DateTime effectiveDate);

        Task<BalanceNote> CreateAsync(Guid userId, DateTime effectiveDate, string content);

        Task<BalanceNote> UpdateAsync(Guid userId, Guid id, string content);
    }
}
