using System;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.EntityFramework.Repositories
{
    public class BalanceNoteRepository : IBalanceNoteRepository
    {
        public Task<BalanceNote> AlterAsync(Guid userOpenId, DateTime effectiveDate, string content)
        {
            throw new NotImplementedException();
        }

        public Task<BalanceNote> CreateAsync(Guid userOpenId, DateTime effectiveDate, string content)
        {
            throw new NotImplementedException();
        }

        public Task<BalanceNote> GetByEffectiveDateAsync(Guid userOpenId, DateTime effectiveDate)
        {
            throw new NotImplementedException();
        }

        public Task<BalanceNote> GetByIdAsync(Guid userOpenId, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BalanceNote> UpdateAsync(Guid userOpenId, Guid id, string content)
        {
            throw new NotImplementedException();
        }
    }
}
