using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Brand;

namespace web.Areas.Admin.Mappers;

public class BrandMappingProfile : Profile
{
    public BrandMappingProfile()
    {
        // Brand Mappings
        CreateMap<Brand, BrandListItemViewModel>()
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products != null ? src.Products.Count : 0));

        CreateMap<Brand, BrandViewModel>().ReverseMap();
    }
}