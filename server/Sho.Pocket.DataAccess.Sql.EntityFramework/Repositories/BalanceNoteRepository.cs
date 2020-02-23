using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.DataAccess.Sql.EntityFramework.Repositories
{
    public class BalanceNoteRepository : IBalanceNoteRepository
    {
        private readonly DbSet<BalanceNote> _set;

        public BalanceNoteRepository(PocketDbContext context)
        {
            _set = context.Set<BalanceNote>();
        }

        public async Task<BalanceNote> GetByEffectiveDateAsync(Guid userOpenId, DateTime effectiveDate)
        {
            return await _set.FirstOrDefaultAsync(n => n.UserOpenId == userOpenId && n.EffectiveDate == effectiveDate.Date);
        }

        public async Task<BalanceNote> GetByIdAsync(Guid userOpenId, Guid id)
        {
            return await _set.FirstOrDefaultAsync(n => n.UserOpenId == userOpenId && n.Id == id);
        }

        public async Task<BalanceNote> AlterAsync(Guid userOpenId, DateTime effectiveDate, string content)
        {
            BalanceNote note =  await _set.FirstOrDefaultAsync(n => n.UserOpenId == userOpenId && n.EffectiveDate == effectiveDate.Date);

            note = note != null 
                ? await UpdateAsync(userOpenId, note.Id, content) 
                : await CreateAsync(userOpenId, effectiveDate, content);

            return note;
        }

        public async Task<BalanceNote> CreateAsync(Guid userOpenId, DateTime effectiveDate, string content)
        {
            BalanceNote note = new BalanceNote(Guid.NewGuid(), effectiveDate, content, userOpenId);

            EntityEntry<BalanceNote> result = await _set.AddAsync(note);

            return result.Entity;
        }

        public async Task<BalanceNote> UpdateAsync(Guid userOpenId, Guid id, string content)
        {
            BalanceNote note = await _set.FirstOrDefaultAsync(n => n.UserOpenId == userOpenId && n.Id == id);
            note.Content = content;
            EntityEntry<BalanceNote> result = _set.Update(note);

            return result.Entity;
        }
    }
}
