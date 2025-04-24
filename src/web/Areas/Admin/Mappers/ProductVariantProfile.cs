using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Product;

namespace web.Areas.Admin.Mappers;

public class ProductVariantProfile : Profile
{
    public ProductVariantProfile()
    {
        // Entity -> ViewModel
        CreateMap<ProductVariant, ProductVariantViewModel>();

        // ViewModel -> Entity
        CreateMap<ProductVariantViewModel, ProductVariant>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.ProductId, opt => opt.Ignore()); // Set by Controller
    }
}