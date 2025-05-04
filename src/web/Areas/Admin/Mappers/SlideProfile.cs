using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.Slide;

namespace web.Areas.Admin.Mappers;

public class SlideProfile : Profile
{
    public SlideProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<Slide, SlideListItemViewModel>();

        // Entity -> ViewModel (GET Edit)
        CreateMap<Slide, SlideViewModel>();

        // ViewModel -> Entity (POST Create / PUT Edit)
        CreateMap<SlideViewModel, Slide>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}