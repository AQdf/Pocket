using AutoMapper;
using Sho.Pocket.Application.ExchangeRates.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Common.Profiles
{
    public class ExchangeRateProfile : Profile
    {
        public ExchangeRateProfile()
        {
            CreateMap<ExchangeRate, ExchangeRateModel>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.EffectiveDate, options => options.MapFrom(src => src.EffectiveDate))
                .ForMember(dest => dest.Value, options => options.MapFrom(src => src.Rate))
                .ForMember(dest => dest.BaseCurrencyId, options => options.MapFrom(src => src.BaseCurrencyId))
                .ForMember(dest => dest.BaseCurrencyName, options => options.MapFrom(src => src.BaseCurrency.Name))
                .ForMember(dest => dest.CounterCurrencyId, options => options.MapFrom(src => src.CounterCurrencyId))
                .ForMember(dest => dest.CounterCurrencyName, options => options.MapFrom(src => src.CounterCurrency.Name));
        }
    }
}