using AutoMapper;
using Sho.Pocket.Application.Currencies.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Common.Profiles
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<Currency, CurrencyViewModel>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, options => options.MapFrom(src => src.Name))
                .ReverseMap();
        }
    }
}
