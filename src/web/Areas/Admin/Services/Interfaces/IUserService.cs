using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface IUserService
{
    Task<IPagedList<UserListItemViewModel>> GetPagedUsersAsync(UserFilterViewModel filter, int pageNumber, int pageSize);

    Task<UserEditViewModel?> GetUserByIdAsync(int id);

    Task<OperationResult<int>> CreateUserAsync(UserCreateViewModel viewModel);

    Task<OperationResult> UpdateUserAsync(UserEditViewModel viewModel, int currentUserId);

    Task<OperationResult> DeleteUserAsync(int userId, int currentUserId);

    Task<bool> IsUsernameUniqueAsync(string username, int? ignoreId = null);

    Task<bool> IsEmailUniqueAsync(string email, int? ignoreId = null);
}
