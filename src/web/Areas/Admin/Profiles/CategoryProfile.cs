using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Models.Category;
using web.Areas.Admin.Requests.Category;

namespace web.Areas.Admin.Profiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryViewModel>()
            .ForMember(
                dest => dest.ParentCategoryName,
                opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : "N/A")
            );

        CreateMap<CategoryCreateRequest, Category>();
        CreateMap<Category, CategoryUpdateRequest>().ReverseMap();
        CreateMap<Category, CategoryDeleteRequest>().ReverseMap();
    }
}