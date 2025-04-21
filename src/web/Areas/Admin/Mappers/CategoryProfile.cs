using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Category;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.Mappers;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Category, CategoryListItemViewModel>()
            .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.Parent != null ? src.Parent.Name : null))
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src =>
                (src.Products != null ? src.Products.Count : 0) +
                (src.Articles != null ? src.Articles.Count : 0) +
                (src.FAQs != null ? src.FAQs.Count : 0)
            ))
            .ForMember(dest => dest.Level, opt => opt.Ignore());

        // --- Mapping for SEO part ---
        CreateMap<Category, SeoViewModel>(); // For GET
        CreateMap<SeoViewModel, Category>() // For POST/PUT - Ignoring non-SEO fields
             .ForMember(dest => dest.Id, opt => opt.Ignore())
             .ForMember(dest => dest.Name, opt => opt.Ignore())
             .ForMember(dest => dest.Slug, opt => opt.Ignore())
             .ForMember(dest => dest.Description, opt => opt.Ignore())
             .ForMember(dest => dest.Icon, opt => opt.Ignore())
             .ForMember(dest => dest.ParentId, opt => opt.Ignore())
             .ForMember(dest => dest.OrderIndex, opt => opt.Ignore())
             .ForMember(dest => dest.IsActive, opt => opt.Ignore())
             .ForMember(dest => dest.Type, opt => opt.Ignore())
             .ForMember(dest => dest.Parent, opt => opt.Ignore())
             .ForMember(dest => dest.Children, opt => opt.Ignore())
             .ForMember(dest => dest.Products, opt => opt.Ignore())
             .ForMember(dest => dest.Articles, opt => opt.Ignore())
             .ForMember(dest => dest.FAQs, opt => opt.Ignore())
             .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
             .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // Entity -> ViewModel (GET Edit)
        CreateMap<Category, CategoryViewModel>()
             .ForMember(dest => dest.ParentCategories, opt => opt.Ignore()) // Dropdown data loaded in controller
             .ForMember(dest => dest.CategoryTypes, opt => opt.Ignore())   // Dropdown data loaded in controller
             .ForMember(dest => dest.Seo, opt => opt.MapFrom(src => src)); // Map SEO part

        // ViewModel -> Entity (POST Create / PUT Edit)
        CreateMap<CategoryViewModel, Category>()
            .ForMember(dest => dest.Parent, opt => opt.Ignore())
            .ForMember(dest => dest.Children, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.Articles, opt => opt.Ignore())
            .ForMember(dest => dest.FAQs, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            // Map nested SeoViewModel onto Category using the specific map defined above
            .AfterMap((src, dest, context) =>
            {
                context.Mapper.Map(src.Seo, dest);
            });
    }
}