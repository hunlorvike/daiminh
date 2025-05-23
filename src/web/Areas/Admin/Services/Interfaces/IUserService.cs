using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface IUserService
{
    Task<IPagedList<UserListItemViewModel>> GetPagedUsersAsync(
        UserFilterViewModel filter,
        int pageNumber,
        int pageSize);

    Task<UserViewModel?> GetUserByIdAsync(int id);

    Task<List<int>> GetSelectedRoleIdsForUserAsync(int userId);

    Task<List<int>> GetSelectedClaimDefinitionIdsForUserAsync(int userId);

    Task<OperationResult<int>> CreateUserAsync(UserViewModel viewModel);

    Task<OperationResult> UpdateUserAsync(UserViewModel viewModel);

    Task<OperationResult> DeleteUserAsync(int id);

    Task<OperationResult> ResetUserPasswordAsync(UserChangePasswordViewModel viewModel);

    Task<OperationResult> ToggleUserActiveStatusAsync(int id, bool isActive);

    Task<OperationResult> ToggleUserLockoutAsync(int id, bool lockAccount);
}