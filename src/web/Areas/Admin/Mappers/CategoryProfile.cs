using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels;

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

        // Entity -> ViewModel (GET Edit)
        CreateMap<Category, CategoryViewModel>()
             .ForMember(dest => dest.ParentCategories, opt => opt.Ignore())
             .ForMember(dest => dest.CategoryTypes, opt => opt.Ignore());

        // ViewModel -> Entity (POST Create / PUT Edit)
        CreateMap<CategoryViewModel, Category>()
            .ForMember(dest => dest.Parent, opt => opt.Ignore())
            .ForMember(dest => dest.Children, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.Articles, opt => opt.Ignore())
            .ForMember(dest => dest.FAQs, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}