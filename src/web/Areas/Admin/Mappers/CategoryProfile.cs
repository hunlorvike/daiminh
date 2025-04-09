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
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src =>
                (src.ProductCategories != null ? src.ProductCategories.Count : 0) +
                (src.ArticleCategories != null ? src.ArticleCategories.Count : 0) +
                (src.ProjectCategories != null ? src.ProjectCategories.Count : 0) +
                (src.GalleryCategories != null ? src.GalleryCategories.Count : 0) + 
                (src.FAQCategories != null ? src.FAQCategories.Count : 0)
            ));

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
}