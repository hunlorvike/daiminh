using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Product;

namespace web.Areas.Admin.Mappers;

public class ProductImageProfile : Profile
{
    public ProductImageProfile()
    {
        // Entity -> ViewModel
        CreateMap<ProductImage, ProductImageViewModel>();

        // ViewModel -> Entity
        CreateMap<ProductImageViewModel, ProductImage>()
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.Product, opt => opt.Ignore())
           .ForMember(dest => dest.ProductId, opt => opt.Ignore());
    }
}
