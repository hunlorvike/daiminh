using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.FAQ;

namespace web.Areas.Admin.Mappers;

public class FAQProfile : Profile
{
    public FAQProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<FAQ, FAQListItemViewModel>();

        // Entity -> ViewModel (For Edit GET)
        CreateMap<FAQ, FAQViewModel>()
             .ForMember(dest => dest.CategoryList, opt => opt.Ignore()); // Ignore dropdown list

        // ViewModel -> Entity (For Create/Edit POST)
        CreateMap<FAQViewModel, FAQ>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
    }
}
