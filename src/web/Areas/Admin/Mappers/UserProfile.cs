using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.User;

namespace web.Areas.Admin.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // Entity -> List ViewModel
        CreateMap<User, UserListItemViewModel>();

        // Entity -> Edit ViewModel
        CreateMap<User, UserEditViewModel>();

        // Create ViewModel -> Entity
        CreateMap<UserCreateViewModel, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        // Edit ViewModel -> Entity (For updating existing entity)
        CreateMap<UserEditViewModel, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
    }
}
