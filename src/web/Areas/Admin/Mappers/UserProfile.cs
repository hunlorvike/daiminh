using AutoMapper;
using domain.Entities;
using web.Areas.Admin.ViewModels.User;

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
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // ID được tạo tự động
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Hash sẽ được tạo ở Controller
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // EditViewModel -> Entity
        CreateMap<UserEditViewModel, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Không bao giờ map mật khẩu từ EditViewModel
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()); // Nên để DbContext tự cập nhật
    }
}