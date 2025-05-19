using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Mappers;

public class ProductImageProfile : Profile
{
    public ProductImageProfile()
    {
        // Entity -> ProductImageViewModel
        CreateMap<ProductImage, ProductImageViewModel>();

        // ProductImageViewModel -> Entity (used in AfterMap)
        CreateMap<ProductImageViewModel, ProductImage>()
            .ForMember(dest => dest.Product, opt => opt.Ignore());
    }
}
