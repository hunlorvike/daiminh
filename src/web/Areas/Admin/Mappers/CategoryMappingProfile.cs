using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Category;

namespace web.Areas.Admin.Mappers;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Category, CategoryListItemViewModel>()
            .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.Parent != null ? src.Parent.Name : null))
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => CalculateItemCount(src)));

        // Entity -> ViewModel (For Edit GET)
        CreateMap<Category, CategoryViewModel>();

        // ViewModel -> Entity (For Create/Edit POST)
        CreateMap<CategoryViewModel, Category>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Parent, opt => opt.Ignore())
            .ForMember(dest => dest.Children, opt => opt.Ignore())
            .ForMember(dest => dest.ProductCategories, opt => opt.Ignore())
            .ForMember(dest => dest.ArticleCategories, opt => opt.Ignore())
            .ForMember(dest => dest.ProjectCategories, opt => opt.Ignore())
            .ForMember(dest => dest.GalleryCategories, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
    }

    // Helper method to calculate item count based on category type
    private int CalculateItemCount(Category category)
    {
        switch (category.Type)
        {
            case shared.Enums.CategoryType.Product:
                return category.ProductCategories?.Count ?? 0;
            case shared.Enums.CategoryType.Article:
                return category.ArticleCategories?.Count ?? 0;
            case shared.Enums.CategoryType.Project:
                return category.ProjectCategories?.Count ?? 0;
            case shared.Enums.CategoryType.Gallery:
                return category.GalleryCategories?.Count ?? 0;
            default:
                return 0;
        }
    }
}