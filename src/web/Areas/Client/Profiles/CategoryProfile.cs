using AutoMapper;
using domain.Entities;
using web.Areas.Client.Models.Category;

namespace web.Areas.Client.Profiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryViewModel>()
            .ForMember(
                dest => dest.ParentCategoryName,
                opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : "N/A")
            );
    }
}