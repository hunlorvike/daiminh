using AutoMapper;
using domain.Entities;
using domain.Entities.Shared;
using web.Areas.Admin.ViewModels.Shared; // Dùng lại SeoViewModel
using web.Areas.Client.ViewModels;

namespace web.Areas.Client.Mappers;

public class ProductClientProfile : Profile
{
    public ProductClientProfile()
    {
        // Mapping cho ProductImage
        CreateMap<ProductImage, ProductImageClientViewModel>();

        // Mapping cho trang chi tiết sản phẩm
        CreateMap<Product, ProductDetailViewModel>()
            .IncludeBase<SeoEntity<int>, SeoViewModel>() // Kế thừa mapping từ SEO
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
            .ForMember(dest => dest.BrandSlug, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Slug : null))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.CategorySlug, opt => opt.MapFrom(src => src.Category != null ? src.Category.Slug : null))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images!.OrderBy(i => i.OrderIndex)));

        // Mapping chung từ SeoEntity sang SeoViewModel (để kế thừa ở trên)
        CreateMap<SeoEntity<int>, SeoViewModel>();
    }
}