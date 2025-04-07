using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.FAQ;

namespace web.Areas.Admin.Mappers;

public class FAQMappingProfile : Profile
{
    public FAQMappingProfile()
    {
        // FAQ Mappings
        CreateMap<FAQCategory, FAQCategoryListItemViewModel>()
            .ForMember(dest => dest.FAQCount, opt => opt.MapFrom(src => src.FAQs != null ? src.FAQs.Count : 0));

        CreateMap<FAQCategory, FAQCategoryViewModel>().ReverseMap();

        CreateMap<FAQ, FAQListItemViewModel>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));

        CreateMap<FAQ, FAQViewModel>().ReverseMap();
    }
}