using AutoMapper;
using core.Entities;
using web.Areas.Client.Models.Account;
using web.Areas.Client.Requests.Account;
using web.Areas.Client.Requests.Auth;

namespace web.Areas.Client.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserViewModel>()
            .ForMember(dest => dest.RoleName,
                opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : string.Empty));

        CreateMap<User, UserRequest>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Role != null ? src.Role.Id : 2));

        CreateMap<LoginRequest, User>()
            .ForMember(dest => dest.PasswordHash,
                opt => opt.MapFrom(src => src.Password ?? string.Empty));

        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password ?? string.Empty));
    }
}
