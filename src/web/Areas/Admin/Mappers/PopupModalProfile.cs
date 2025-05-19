using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Mappers;

public class PopupModalProfile : Profile
{
    public PopupModalProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<PopupModal, PopupModalListItemViewModel>();

        // Entity -> ViewModel (GET Edit)
        CreateMap<PopupModal, PopupModalViewModel>();

        // ViewModel -> Entity (POST Create / PUT Edit)
        CreateMap<PopupModalViewModel, PopupModal>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}