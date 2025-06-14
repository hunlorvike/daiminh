using AutoMapper;
using AutoRegister;
using domain.Entities;
using Microsoft.AspNetCore.Identity;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public class ProfileService : IProfileService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public ProfileService(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ProfileViewModel?> GetProfileAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return _mapper.Map<ProfileViewModel>(user);
    }

    public async Task<OperationResult> UpdateProfileAsync(string userId, ProfileViewModel viewModel)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return OperationResult.FailureResult("Không tìm thấy người dùng.");
        }

        user.FullName = viewModel.FullName;
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded
            ? OperationResult.SuccessResult("Cập nhật hồ sơ thành công.")
            : OperationResult.FailureResult("Lỗi khi cập nhật hồ sơ.", result.Errors.Select(e => e.Description).ToList());
    }

    public async Task<OperationResult> ChangePasswordAsync(string userId, ChangePasswordViewModel viewModel)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return OperationResult.FailureResult("Không tìm thấy người dùng.");
        }

        var result = await _userManager.ChangePasswordAsync(user, viewModel.CurrentPassword, viewModel.NewPassword);

        return result.Succeeded
            ? OperationResult.SuccessResult("Đổi mật khẩu thành công.")
            : OperationResult.FailureResult("Lỗi khi đổi mật khẩu.", result.Errors.Select(e => e.Description).ToList());
    }
}