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
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Sẽ xử lý hash riêng
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());

        // Edit ViewModel -> Entity (Dùng để cập nhật entity đã tồn tại)
        CreateMap<UserEditViewModel, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Không cập nhật mật khẩu ở đây
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id không được map từ ViewModel
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore());
    }
}