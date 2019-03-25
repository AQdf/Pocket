using Sho.Pocket.Domain.Entities;
using System;

namespace Sho.Pocket.Application.Currencies.Models
{
    public class CurrencyViewModel
    {
        public CurrencyViewModel(Currency currency)
        {
            Id = currency.Id;
            Name = currency.Name;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
