using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Category;

namespace web.Areas.Admin.Mappers;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryListItemViewModel>()
             .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.Parent != null ? src.Parent.Name : null))
             .ForMember(dest => dest.ChildrenCount, opt => opt.MapFrom(src => src.Children != null ? src.Children.Count : 0))
             // Calculate ItemCount based on all possible related collections
             .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src =>
                 (src.ProductCategories != null ? src.ProductCategories.Count : 0) +
                 (src.ArticleCategories != null ? src.ArticleCategories.Count : 0) +
                 (src.ProjectCategories != null ? src.ProjectCategories.Count : 0) + // Assuming ProjectCategories exists
                 (src.GalleryCategories != null ? src.GalleryCategories.Count : 0)
             ));

        CreateMap<Category, CategoryViewModel>() // Added Parent Name mapping
             .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.Parent != null ? src.Parent.Name : null))
             .ReverseMap(); // Keep ReverseMap if needed for saving

        CreateMap<Category, CategorySelectViewModel>() // Renamed from CategoryParentViewModel for clarity
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
             .ForMember(dest => dest.Level, opt => opt.Ignore()); // Level usually set during recursive fetch

    }
}
