using AutoMapper;
using Sho.Pocket.Application.Assets.Models;
using Sho.Pocket.Domain.Entities;

namespace Sho.Pocket.Application.Common.Profiles
{
    public class AssetProfile : Profile
    {
        public AssetProfile()
        {
            CreateMap<Asset, AssetViewModel>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, options => options.MapFrom(src => src.Name))
                .ForMember(dest => dest.CurrencyId, options => options.MapFrom(src => src.CurrencyId))
                .ForMember(dest => dest.CurrencyName, options => options.MapFrom(src => src.CurrencyName))
                .ForMember(dest => dest.IsActive, options => options.MapFrom(src => src.IsActive))
                .ReverseMap();
        }
    }
}
