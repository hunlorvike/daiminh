using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Product;

namespace web.Areas.Admin.Mappers;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Product, ProductListItemViewModel>()
            .ForMember(dest => dest.ProductTypeName, opt => opt.MapFrom(src => src.ProductType != null ? src.ProductType.Name : null))
            .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.Name : null))
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => // Find main image path
                src.Images != null && src.Images.Any(i => i.IsMain)
                    ? src.Images.OrderBy(i => i.OrderIndex).First(i => i.IsMain).ImageUrl
                    : src.Images != null && src.Images.Any()
                        ? src.Images.OrderBy(i => i.OrderIndex).First().ImageUrl
                        : null
            ))
             .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt ?? src.CreatedAt)); // Map UpdatedAt


        // Entity -> ViewModel (For Edit GET)
        CreateMap<Product, ProductViewModel>()
            .ForMember(dest => dest.SelectedCategoryIds, opt => opt.MapFrom(src => src.ProductCategories != null ? src.ProductCategories.Select(pc => pc.CategoryId).ToList() : new List<int>()))
            .ForMember(dest => dest.SelectedTagIds, opt => opt.MapFrom(src => src.ProductTags != null ? src.ProductTags.Select(pt => pt.TagId).ToList() : new List<int>()))
            // Let other profiles handle nested Images and Variants lists
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
            .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants))
            // Ignore dropdown lists - populated by controller
            .ForMember(dest => dest.ProductTypeList, opt => opt.Ignore())
            .ForMember(dest => dest.BrandList, opt => opt.Ignore())
            .ForMember(dest => dest.CategoryList, opt => opt.Ignore())
            .ForMember(dest => dest.TagList, opt => opt.Ignore())
            .ForMember(dest => dest.StatusList, opt => opt.Ignore());


        // ViewModel -> Entity (For Create/Edit POST)
        CreateMap<ProductViewModel, Product>()
            // Ignore collections/navigation properties - handled manually in Controller
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.Variants, opt => opt.Ignore())
            .ForMember(dest => dest.ProductCategories, opt => opt.Ignore())
            .ForMember(dest => dest.ProductTags, opt => opt.Ignore())
            .ForMember(dest => dest.ProductType, opt => opt.Ignore())
            .ForMember(dest => dest.Brand, opt => opt.Ignore())
            .ForMember(dest => dest.ProjectProducts, opt => opt.Ignore())
            .ForMember(dest => dest.ArticleProducts, opt => opt.Ignore())
            // Ignore other fields not directly on ViewModel or handled by BaseEntity/SeoEntity
            .ForMember(dest => dest.ViewCount, opt => opt.Ignore()) // ViewCount not edited here
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
    }
}
