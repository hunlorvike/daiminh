using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // Entity -> ListItemViewModel
        CreateMap<User, UserListItemViewModel>();

        // Entity -> EditViewModel
        CreateMap<User, UserEditViewModel>();

        // CreateViewModel -> Entity
        CreateMap<UserCreateViewModel, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        // EditViewModel -> Entity
        CreateMap<UserEditViewModel, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
    }
}