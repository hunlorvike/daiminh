using AutoMapper;
using domain.Entities;
using web.Areas.Admin.Requests.Category;

namespace web.Areas.Admin.Profiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryCreateRequest, Category>();
        CreateMap<Category, CategoryUpdateRequest>().ReverseMap();
        CreateMap<Category, CategoryDeleteRequest>().ReverseMap();
    }
}