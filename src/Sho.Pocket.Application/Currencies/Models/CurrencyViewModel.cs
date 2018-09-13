using System;

namespace Sho.Pocket.Application.Currencies.Models
{
    public class CurrencyViewModel
    {
        public CurrencyViewModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
