using AutoMapper;
using Sho.Pocket.Application.Balances.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Common.Profiles
{
    public class BalanceProfile : Profile
    {
        public BalanceProfile()
        {
            CreateMap<Balance, BalanceViewModel>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.AssetId, options => options.MapFrom(src => src.AssetId))
                .ForMember(dest => dest.EffectiveDate, options => options.MapFrom(src => src.EffectiveDate))
                .ForMember(dest => dest.Value, options => options.MapFrom(src => src.Value))
                .ForMember(dest => dest.ExchangeRateId, options => options.MapFrom(src => src.ExchangeRateId))
                .ForMember(dest => dest.ExchangeRateValue, options => options.MapFrom(src => src.ExchangeRate.Rate))
                .ForMember(dest => dest.Asset, options => options.MapFrom(src => src.Asset))
                .ForMember(dest => dest.DefaultCurrencyValue, options => options.MapFrom(src => src.Value * src.ExchangeRate.Rate));

            CreateMap<BalanceViewModel, Balance>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.AssetId, options => options.MapFrom(src => src.AssetId))
                .ForMember(dest => dest.EffectiveDate, options => options.MapFrom(src => src.EffectiveDate))
                .ForMember(dest => dest.Value, options => options.MapFrom(src => src.Value))
                .ForMember(dest => dest.ExchangeRateId, options => options.MapFrom(src => src.ExchangeRateId))
                .ForMember(dest => dest.Asset, options => options.MapFrom(src => src.Asset));
        }
    }
}