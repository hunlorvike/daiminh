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
                (src.Products != null ? src.Products.Count : 0) +
                (src.Articles != null ? src.Articles.Count : 0) +
                (src.Projects != null ? src.Projects.Count : 0) +
                (src.Galleries != null ? src.Galleries.Count : 0) +
                (src.FAQs != null ? src.FAQs.Count : 0)
            ));

        // Entity -> ViewModel (For Edit GET)
        CreateMap<Category, CategoryViewModel>();

        // ViewModel -> Entity (For Create/Edit POST)
        CreateMap<CategoryViewModel, Category>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Parent, opt => opt.Ignore())
            .ForMember(dest => dest.Children, opt => opt.Ignore())
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.Articles, opt => opt.Ignore())
            .ForMember(dest => dest.Projects, opt => opt.Ignore())
            .ForMember(dest => dest.Galleries, opt => opt.Ignore())
            .ForMember(dest => dest.FAQs, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
    }
}