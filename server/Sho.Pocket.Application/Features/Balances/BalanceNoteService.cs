using System;
using System.Threading.Tasks;
using Sho.Pocket.Core.DataAccess;
using Sho.Pocket.Core.Features.Balances.Abstractions;
using Sho.Pocket.Core.Features.Balances.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Features.Balances
{
    public class BalanceNoteService : IBalanceNoteService
    {
        private readonly IBalanceNoteRepository _balanceNoteRepository;

        public BalanceNoteService(IBalanceNoteRepository balanceNoteRepository)
        {
            _balanceNoteRepository = balanceNoteRepository;
        }

        public async Task<BalanceNoteViewModel> GetNoteByIdAsync(Guid userOpenId, Guid id)
        {
            BalanceNote note = await _balanceNoteRepository.GetByIdAsync(userOpenId, id);
            BalanceNoteViewModel result = new BalanceNoteViewModel(note);

            return result;
        }

        public async Task<BalanceNoteViewModel> GetNoteByEffectiveDateAsync(Guid userOpenId, DateTime effectiveDate)
        {
            BalanceNote note = await _balanceNoteRepository.GetByEffectiveDateAsync(userOpenId, effectiveDate);

            BalanceNoteViewModel result = null;

            if (note != null)
            {
                result = new BalanceNoteViewModel(note);
            }

            return result;
        }

        public async Task<BalanceNoteViewModel> AlterNoteAsync(Guid userOpenId, DateTime effectiveDate, string content)
        {
            BalanceNote note = await _balanceNoteRepository.AlterAsync(userOpenId, effectiveDate, content);
            BalanceNoteViewModel result = new BalanceNoteViewModel(note);

            return result;
        }

        public async Task<BalanceNoteViewModel> AddNoteAsync(Guid userOpenId, DateTime effectiveDate, string content)
        {
            BalanceNote note = await _balanceNoteRepository.CreateAsync(userOpenId, effectiveDate, content);
            BalanceNoteViewModel result = new BalanceNoteViewModel(note);

            return result;
        }

        public async Task<BalanceNoteViewModel> UpdateNoteAsync(Guid userOpenId, Guid id, string content)
        {
            BalanceNote note = await _balanceNoteRepository.UpdateAsync(userOpenId, id, content);
            BalanceNoteViewModel result = new BalanceNoteViewModel(note);

            return result;
        }
    }
}
