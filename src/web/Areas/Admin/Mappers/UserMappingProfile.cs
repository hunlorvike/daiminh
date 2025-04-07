using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.User;

namespace web.Areas.Admin.Mappers;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserListItemViewModel>();

        CreateMap<User, UserViewModel>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore());

        CreateMap<UserViewModel, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
    }
}
