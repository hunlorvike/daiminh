using shared.Models;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Services.Interfaces;

public interface IProfileService
{
    Task<ProfileViewModel?> GetProfileAsync(string userId);
    Task<OperationResult> UpdateProfileAsync(string userId, ProfileViewModel viewModel);
    Task<OperationResult> ChangePasswordAsync(string userId, ChangePasswordViewModel viewModel);
}