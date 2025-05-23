using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // Entity -> ListItemViewModel (cho trang Index)
        CreateMap<User, UserListItemViewModel>()
            .ForMember(dest => dest.RoleNames, opt => opt.Ignore());

        // Entity -> ViewModel (cho form Edit/GET)
        CreateMap<User, UserViewModel>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore())
            .ForMember(dest => dest.SelectedRoleIds, opt => opt.Ignore())
            .ForMember(dest => dest.AvailableRoles, opt => opt.Ignore())
            .ForMember(dest => dest.SelectedClaimDefinitionIds, opt => opt.Ignore())
            .ForMember(dest => dest.AvailableClaimDefinitions, opt => opt.Ignore())
            .ForMember(dest => dest.HasPassword, opt => opt.MapFrom(src => src.PasswordHash != null));

        // ViewModel -> Entity (cho form Create/Edit/POST)
        CreateMap<UserViewModel, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.Email.ToUpperInvariant()))
            .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpperInvariant()))
            .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
            .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
            .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
            .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
            .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
            .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
            .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
            .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
            .ForMember(dest => dest.ReviewsWritten, opt => opt.Ignore());
    }
}