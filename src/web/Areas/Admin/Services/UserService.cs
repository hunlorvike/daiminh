using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoRegister;
using domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IMapper mapper,
        ILogger<UserService> logger)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IPagedList<UserListItemViewModel>> GetPagedUsersAsync(UserFilterViewModel filter, int pageNumber, int pageSize)
    {
        // UserManager.Users cung cấp IQueryable<User>
        IQueryable<User> query = _userManager.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(u =>
                (u.UserName != null && u.UserName.ToLower().Contains(lowerSearchTerm)) ||
                (u.Email != null && u.Email.ToLower().Contains(lowerSearchTerm)) ||
                (u.FullName != null && u.FullName.ToLower().Contains(lowerSearchTerm)));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(u => u.IsActive == filter.IsActive.Value);
        }

        query = query.OrderBy(u => u.UserName); // Sắp xếp theo UserName

        IPagedList<UserListItemViewModel> usersPaged = await query
            .ProjectTo<UserListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        return usersPaged;
    }

    public async Task<UserEditViewModel?> GetUserByIdAsync(int id)
    {
        User? user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return null;

        var viewModel = _mapper.Map<UserEditViewModel>(user);

        // Nếu bạn muốn load roles của user:
        // var roles = await _userManager.GetRolesAsync(user);
        // viewModel.SelectedRoles = roles.ToList(); // Cần thêm SelectedRoles vào UserEditViewModel

        return viewModel;
    }

    public async Task<OperationResult<int>> CreateUserAsync(UserCreateViewModel viewModel)
    {
        var existingUserByUserName = await _userManager.FindByNameAsync(viewModel.UserName);
        if (existingUserByUserName != null)
        {
            return OperationResult<int>.FailureResult("Tên đăng nhập đã tồn tại.");
        }

        if (!string.IsNullOrEmpty(viewModel.Email))
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(viewModel.Email);
            if (existingUserByEmail != null)
            {
                return OperationResult<int>.FailureResult("Email đã tồn tại.");
            }
        }

        var user = _mapper.Map<User>(viewModel);
        user.EmailConfirmed = true;

        IdentityResult result = await _userManager.CreateAsync(user, viewModel.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("Created User: ID={Id}, Username={Username}", user.Id, user.UserName);

            if (viewModel.SelectedRoles != null && viewModel.SelectedRoles.Any())
            {
                var validRoles = new List<string>();
                foreach (var roleName in viewModel.SelectedRoles)
                {
                    if (await _roleManager.RoleExistsAsync(roleName))
                    {
                        validRoles.Add(roleName);
                    }
                    else
                    {
                        _logger.LogWarning("Role '{RoleName}' selected for user '{Username}' does not exist.", roleName, user.UserName);
                    }
                }

                if (validRoles.Any())
                {
                    var addToRolesResult = await _userManager.AddToRolesAsync(user, validRoles);
                    if (!addToRolesResult.Succeeded)
                    {
                        _logger.LogWarning("Could not add roles to user {UserId}. Errors: {Errors}", user.Id, string.Join(", ", addToRolesResult.Errors.Select(e => e.Description)));
                    }
                }
            }
            return OperationResult<int>.SuccessResult(user.Id, $"Thêm người dùng '{user.UserName}' thành công.");
        }
        else
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            _logger.LogError("Lỗi khi tạo người dùng {Username}: {Errors}", viewModel.UserName, string.Join("; ", errors));
            return OperationResult<int>.FailureResult(errors.FirstOrDefault() ?? "Lỗi khi tạo người dùng.", errors);
        }
    }

    public async Task<OperationResult> UpdateUserAsync(UserEditViewModel viewModel, int currentUserId)
    {
        var user = await _userManager.FindByIdAsync(viewModel.Id.ToString());
        if (user == null)
        {
            _logger.LogWarning("User not found for update. ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult("Không tìm thấy người dùng để cập nhật.");
        }

        // Kiểm tra nếu UserName thay đổi và đã tồn tại
        if (!user.UserName.Equals(viewModel.UserName, StringComparison.OrdinalIgnoreCase))
        {
            var existingUserByUserName = await _userManager.FindByNameAsync(viewModel.UserName);
            if (existingUserByUserName != null && existingUserByUserName.Id != user.Id)
            {
                return OperationResult.FailureResult("Tên đăng nhập đã tồn tại.");
            }
        }

        // Kiểm tra nếu Email thay đổi và đã tồn tại
        if (user.Email == null || !user.Email.Equals(viewModel.Email, StringComparison.OrdinalIgnoreCase))
        {
            if (!string.IsNullOrEmpty(viewModel.Email))
            {
                var existingUserByEmail = await _userManager.FindByEmailAsync(viewModel.Email);
                if (existingUserByEmail != null && existingUserByEmail.Id != user.Id)
                {
                    return OperationResult.FailureResult("Email đã tồn tại.");
                }
            }
        }

        if (!viewModel.IsActive && (user.Id == currentUserId || user.Id == 1))
        {
            _logger.LogWarning("Attempt to deactivate self ({CurrentUserId}) or primary admin (ID=1)", currentUserId);
            return OperationResult.FailureResult("Không thể hủy kích hoạt chính mình hoặc tài khoản quản trị viên chính.");
        }

        user.FullName = viewModel.FullName;
        user.IsActive = viewModel.IsActive;
        if (user.UserName == null || !user.UserName.Equals(viewModel.UserName, StringComparison.OrdinalIgnoreCase))
        {
            var setUserNameResult = await _userManager.SetUserNameAsync(user, viewModel.UserName);
            if (!setUserNameResult.Succeeded)
            {
                var errors = setUserNameResult.Errors.Select(e => e.Description).ToList();
                return OperationResult.FailureResult(
                    message: errors.FirstOrDefault() ?? "Lỗi khi cập nhật tên đăng nhập.",
                    errors: errors
                );
            }
        }

        if (user.Email == null || !user.Email.Equals(viewModel.Email, StringComparison.OrdinalIgnoreCase))
        {
            if (!string.IsNullOrEmpty(viewModel.Email))
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, viewModel.Email);
                if (!setEmailResult.Succeeded)
                {
                    var errors = setEmailResult.Errors.Select(e => e.Description).ToList();
                    return OperationResult.FailureResult(
                        message: errors.FirstOrDefault() ?? "Lỗi khi cập nhật email.",
                        errors: errors
                    );
                }
            }
            else
            {
                user.Email = null;
                user.NormalizedEmail = null;
                user.EmailConfirmed = false;
            }
        }


        IdentityResult updateResult = await _userManager.UpdateAsync(user);

        if (updateResult.Succeeded)
        {
            _logger.LogInformation("Updated User: ID={Id}, Username={Username}, IsActive={IsActive}", user.Id, user.UserName, user.IsActive);

            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToRemove = currentRoles.Except(viewModel.SelectedRoles ?? Enumerable.Empty<string>()).ToList();
            var rolesToAdd = (viewModel.SelectedRoles ?? Enumerable.Empty<string>()).Except(currentRoles).ToList();

            if (rolesToRemove.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                {
                    _logger.LogWarning("Could not remove roles from user {UserId}. Errors: {Errors}", user.Id, string.Join(", ", removeResult.Errors.Select(e => e.Description)));
                }
            }
            if (rolesToAdd.Any())
            {
                var validRolesToAdd = new List<string>();
                foreach (var roleName in rolesToAdd)
                {
                    if (await _roleManager.RoleExistsAsync(roleName))
                    {
                        validRolesToAdd.Add(roleName);
                    }
                    else
                    {
                        _logger.LogWarning("Role '{RoleName}' selected for user '{Username}' does not exist and cannot be added.", roleName, user.UserName);
                    }
                }
                if (validRolesToAdd.Any())
                {
                    var addResult = await _userManager.AddToRolesAsync(user, validRolesToAdd);
                    if (!addResult.Succeeded)
                    {
                        _logger.LogWarning("Could not add roles to user {UserId}. Errors: {Errors}", user.Id, string.Join(", ", addResult.Errors.Select(e => e.Description)));
                    }
                }
            }

            return OperationResult.SuccessResult($"Cập nhật người dùng '{user.UserName}' thành công.");
        }
        else
        {
            var errors = updateResult.Errors.Select(e => e.Description).ToList();
            _logger.LogError("Lỗi khi cập nhật người dùng ID {Id}, Username {Username}: {Errors}", viewModel.Id, viewModel.UserName, string.Join("; ", errors));
            return OperationResult.FailureResult(errors.FirstOrDefault() ?? "Lỗi khi cập nhật người dùng.", errors);
        }
    }

    public async Task<OperationResult> DeleteUserAsync(int userId, int currentUserId)
    {
        if (userId == currentUserId)
        {
            _logger.LogWarning("Attempt to delete self ({CurrentUserId})", currentUserId);
            return OperationResult.FailureResult("Bạn không thể xóa tài khoản của chính mình.");
        }

        if (userId == 1)
        {
            _logger.LogWarning("Attempt to delete primary admin (ID=1) by user ({CurrentUserId})", currentUserId);
            return OperationResult.FailureResult("Không thể xóa tài khoản quản trị viên chính.");
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            _logger.LogWarning("User not found for delete. ID: {Id}", userId);
            return OperationResult.FailureResult("Không tìm thấy người dùng.");
        }

        string username = user.UserName ?? "N/A";
        IdentityResult result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
        {
            _logger.LogInformation("Deleted User: ID={Id}, Username={Username}", userId, username);
            return OperationResult.SuccessResult($"Xóa người dùng '{username}' thành công.");
        }
        else
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            _logger.LogError("Lỗi khi xóa người dùng ID {Id}: {Errors}", userId, string.Join("; ", errors));
            return OperationResult.FailureResult(errors.FirstOrDefault() ?? $"Không thể xóa người dùng '{username}'.", errors);
        }
    }

    public async Task<bool> IsUsernameUniqueAsync(string username, int? ignoreId = null)
    {
        if (string.IsNullOrWhiteSpace(username)) return false;
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return false;
        if (ignoreId.HasValue && user.Id == ignoreId.Value) return false;
        return true;
    }

    public async Task<bool> IsEmailUniqueAsync(string email, int? ignoreId = null)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;
        if (ignoreId.HasValue && user.Id == ignoreId.Value) return false;
        return true;
    }
}