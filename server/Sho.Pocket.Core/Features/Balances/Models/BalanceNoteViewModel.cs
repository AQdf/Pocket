using System;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Core.Features.Balances.Models
{
    public class BalanceNoteViewModel
    {
        public BalanceNoteViewModel(BalanceNote note)
        {
            Id = note.Id;
            EffectiveDate = note.EffectiveDate;
            Content = note.Content;
        }

        public Guid Id { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string Content { get; set; }
    }
}
