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

        private readonly IUnitOfWork _unitOfWork;

        public BalanceNoteService(IBalanceNoteRepository balanceNoteRepository, IUnitOfWork unitOfWork)
        {
            _balanceNoteRepository = balanceNoteRepository;
            _unitOfWork = unitOfWork;
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
            BalanceNote note = await _balanceNoteRepository.GetByEffectiveDateAsync(userOpenId, effectiveDate);

            note = note != null
                ? await _balanceNoteRepository.UpdateAsync(userOpenId, note.Id, content)
                : await _balanceNoteRepository.CreateAsync(userOpenId, effectiveDate, content);

            await _unitOfWork.SaveChangesAsync();
            BalanceNoteViewModel result = new BalanceNoteViewModel(note);

            return result;
        }
    }
}
