using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.FAQ;

namespace web.Areas.Admin.Mappers;

public class FAQProfile : Profile
{
    public FAQProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<FAQ, FAQListItemViewModel>()
            .ForMember(dest => dest.CategoryName,
                      opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));
        
        // Entity -> ViewModel (For Edit GET)
        CreateMap<FAQ, FAQViewModel>()
             .ForMember(dest => dest.CategoryList, opt => opt.Ignore());

        // ViewModel -> Entity (For Create/Edit POST)
        CreateMap<FAQViewModel, FAQ>();
    }
}