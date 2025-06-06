using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoRegister;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using shared.Models;
using System.Security.Claims;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public UserService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<UserService> logger,
        UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
    }

    public async Task<IPagedList<UserListItemViewModel>> GetPagedUsersAsync(
        UserFilterViewModel filter,
        int pageNumber,
        int pageSize)
    {
        IQueryable<User> query = _userManager.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(u => (u.FullName != null && u.FullName.ToLower().Contains(lowerSearchTerm))
                                  || (u.Email != null && u.Email.ToLower().Contains(lowerSearchTerm)));
        }

        if (filter.IsActiveFilter.HasValue)
        {
            query = query.Where(u => u.IsActive == filter.IsActiveFilter.Value);
        }

        if (filter.RoleIdFilter.HasValue && filter.RoleIdFilter.Value > 0)
        {
            query = query.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == filter.RoleIdFilter.Value));
        }

        query = query.OrderBy(u => u.Email);

        var pagedUsers = await query
            .ProjectTo<UserListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        foreach (var userItem in pagedUsers)
        {
            var user = await _userManager.FindByIdAsync(userItem.Id.ToString());
            if (user != null)
            {
                var roleNames = await _userManager.GetRolesAsync(user);
                userItem.RoleNames.AddRange(roleNames);
            }
        }

        return pagedUsers;
    }

    public async Task<UserViewModel?> GetUserByIdAsync(int id)
    {
        User? user = await _userManager.Users
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return null;

        var viewModel = _mapper.Map<UserViewModel>(user);

        viewModel.SelectedRoleIds = await GetSelectedRoleIdsForUserAsync(id);

        viewModel.SelectedClaimDefinitionIds = await GetSelectedClaimDefinitionIdsForUserAsync(id);

        return viewModel;
    }

    public async Task<List<int>> GetSelectedRoleIdsForUserAsync(int userId)
    {
        return await _context.Set<UserRole>()
                             .AsNoTracking()
                             .Where(ur => ur.UserId == userId)
                             .Select(ur => ur.RoleId)
                             .ToListAsync();
    }

    public async Task<List<int>> GetSelectedClaimDefinitionIdsForUserAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return [];

        var currentUserClaims = await _userManager.GetClaimsAsync(user);
        if (currentUserClaims == null) return [];

        var allClaimDefinitions = await _context.ClaimDefinitions.AsNoTracking().ToListAsync();
        var claimDefDict = allClaimDefinitions.ToDictionary(cd => (cd.Type, cd.Value), cd => cd.Id);

        var selectedIds = new List<int>();
        foreach (var claim in currentUserClaims)
        {
            if (claim.Type == "Permission" && claimDefDict.TryGetValue((claim.Type, claim.Value), out int claimDefId))
            {
                selectedIds.Add(claimDefId);
            }
        }
        return selectedIds;
    }

    public async Task<OperationResult<int>> CreateUserAsync(UserViewModel viewModel)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            User user = _mapper.Map<User>(viewModel);
            user.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(user, viewModel.Password ?? "");

            if (result.Succeeded)
            {
                await SyncUserRolesAsync(user.Id, viewModel.SelectedRoleIds);
                await SyncUserClaimsAsync(user.Id, viewModel.SelectedClaimDefinitionIds);

                await transaction.CommitAsync();

                _logger.LogInformation("Created User: ID={Id}, Email={Email}, FullName={FullName}", user.Id, user.Email, user.FullName);
                return OperationResult<int>.SuccessResult(user.Id, $"Thêm người dùng '{user.Email}' thành công.");
            }
            else
            {
                await transaction.RollbackAsync();
                List<string> errors = result.Errors.Select(e => e.Description).ToList();
                _logger.LogError("Failed to create user '{Email}': {Errors}", viewModel.Email, string.Join(", ", errors));
                return OperationResult<int>.FailureResult(message: "Không thể thêm người dùng.", errors: errors);
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Exception occurred while creating user '{Email}'", viewModel.Email);
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi khi thêm người dùng.");
        }
    }

    public async Task<OperationResult> UpdateUserAsync(UserViewModel viewModel)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            User? user = await _userManager.FindByIdAsync(viewModel.Id.ToString());
            if (user == null)
            {
                _logger.LogWarning("User not found for update. ID: {Id}", viewModel.Id);
                return OperationResult.FailureResult("Không tìm thấy người dùng để cập nhật.");
            }

            string oldEmail = user.Email ?? string.Empty;
            _mapper.Map(viewModel, user);

            if (!string.Equals(oldEmail, viewModel.Email, StringComparison.OrdinalIgnoreCase))
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, viewModel.Email);
                if (!setEmailResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    List<string> emailErrors = setEmailResult.Errors.Select(e => e.Description).ToList();
                    _logger.LogError("Failed to update user email '{Email}': {Errors}", viewModel.Email, string.Join(", ", emailErrors));
                    return OperationResult.FailureResult(message: "Không thể cập nhật email người dùng.", errors: emailErrors);
                }
                var setUserNameResult = await _userManager.SetUserNameAsync(user, viewModel.Email);
                if (!setUserNameResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    List<string> userNameErrors = setUserNameResult.Errors.Select(e => e.Description).ToList();
                    _logger.LogError("Failed to update user username '{Email}': {Errors}", viewModel.Email, string.Join(", ", userNameErrors));
                    return OperationResult.FailureResult(message: "Không thể cập nhật username người dùng.", errors: userNameErrors);
                }
            }
            user.IsActive = viewModel.IsActive;

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                await SyncUserRolesAsync(user.Id, viewModel.SelectedRoleIds);
                await SyncUserClaimsAsync(user.Id, viewModel.SelectedClaimDefinitionIds);

                await transaction.CommitAsync();

                _logger.LogInformation("Updated User: ID={Id}, OldEmail={OldEmail}, NewEmail={NewEmail}", user.Id, oldEmail, user.Email);
                return OperationResult.SuccessResult($"Cập nhật người dùng '{user.Email}' thành công.");
            }
            else
            {
                await transaction.RollbackAsync();
                List<string> errors = updateResult.Errors.Select(e => e.Description).ToList();
                _logger.LogError("Failed to update user '{Email}': {Errors}", viewModel.Email, string.Join(", ", errors));
                return OperationResult.FailureResult(message: "Không thể cập nhật người dùng.", errors: errors);
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Exception occurred while updating user '{Email}'", viewModel.Email);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi khi cập nhật người dùng.");
        }
    }

    public async Task<OperationResult> DeleteUserAsync(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            User? user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return OperationResult.FailureResult("Không tìm thấy người dùng.");
            }

            string userEmail = user.Email ?? string.Empty;

            var userRoles = await _context.UserRoles.Where(ur => ur.UserId == id).ToListAsync();
            if (userRoles.Count != 0)
            {
                _context.UserRoles.RemoveRange(userRoles);
                await _context.SaveChangesAsync();
            }

            var userClaims = await _context.UserClaims.Where(uc => uc.UserId == id).ToListAsync();
            if (userClaims.Count != 0)
            {
                _context.UserClaims.RemoveRange(userClaims);
                await _context.SaveChangesAsync();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                await transaction.CommitAsync();
                _logger.LogInformation("Deleted User: ID={Id}, Email={Email}", id, userEmail);
                return OperationResult.SuccessResult($"Xóa người dùng '{userEmail}' thành công.");
            }
            else
            {
                await transaction.RollbackAsync();
                List<string> errors = result.Errors.Select(e => e.Description).ToList();
                _logger.LogError("Failed to delete user '{Email}': {Errors}", userEmail, string.Join(", ", errors));
                return OperationResult.FailureResult(message: "Không thể xóa người dùng.", errors: errors);
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Exception occurred while deleting user '{Id}'", id);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi khi xóa người dùng.");
        }
    }

    public async Task<OperationResult> ResetUserPasswordAsync(UserChangePasswordViewModel viewModel)
    {
        User? user = await _userManager.FindByIdAsync(viewModel.UserId.ToString());
        if (user == null)
        {
            _logger.LogWarning("User not found for password reset. ID: {Id}", viewModel.UserId);
            return OperationResult.FailureResult("Không tìm thấy người dùng để đặt lại mật khẩu.");
        }

        var removePasswordResult = await _userManager.RemovePasswordAsync(user);
        if (!removePasswordResult.Succeeded && removePasswordResult.Errors.Any(e => e.Code != "PasswordRequiresNonAlphanumeric" && e.Code != "PasswordRequiresDigit" && e.Code != "PasswordRequiresLower" && e.Code != "PasswordRequiresUpper" && e.Code != "PasswordTooShort"))
        {
            List<string> errors = removePasswordResult.Errors.Select(e => e.Description).ToList();
            _logger.LogError("Failed to remove current password for user {UserId}: {Errors}", viewModel.UserId, string.Join(", ", errors));
            return OperationResult.FailureResult(message: "Lỗi khi xóa mật khẩu cũ.", errors: errors);
        }

        var addPasswordResult = await _userManager.AddPasswordAsync(user, viewModel.NewPassword);

        if (addPasswordResult.Succeeded)
        {
            _logger.LogInformation("Reset password for User: ID={Id}, Email={Email}", user.Id, user.Email);
            return OperationResult.SuccessResult($"Đặt lại mật khẩu cho người dùng '{user.Email}' thành công.");
        }
        else
        {
            List<string> errors = addPasswordResult.Errors.Select(e => e.Description).ToList();
            _logger.LogError("Failed to reset password for user '{Email}': {Errors}", user.Email, string.Join(", ", errors));
            return OperationResult.FailureResult(message: "Không thể đặt lại mật khẩu.", errors: errors);
        }
    }

    public async Task<OperationResult> ToggleUserActiveStatusAsync(int id, bool isActive)
    {
        User? user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            _logger.LogWarning("User not found for toggling active status. ID: {Id}", id);
            return OperationResult.FailureResult("Không tìm thấy người dùng.");
        }

        user.IsActive = isActive;
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            string status = isActive ? "kích hoạt" : "vô hiệu hóa";
            _logger.LogInformation("Toggled active status for User: ID={Id}, Email={Email}, IsActive={IsActive}", user.Id, user.Email, isActive);
            return OperationResult.SuccessResult($"Đã {status} người dùng '{user.Email}' thành công.");
        }
        else
        {
            List<string> errors = result.Errors.Select(e => e.Description).ToList();
            _logger.LogError("Failed to toggle active status for user '{Email}': {Errors}", user.Email, string.Join(", ", errors));
            return OperationResult.FailureResult(message: $"Không thể {(isActive ? "kích hoạt" : "vô hiệu hóa")} người dùng.", errors: errors);
        }
    }

    public async Task<OperationResult> ToggleUserLockoutAsync(int id, bool lockAccount)
    {
        User? user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            _logger.LogWarning("User not found for toggling lockout. ID: {Id}", id);
            return OperationResult.FailureResult("Không tìm thấy người dùng.");
        }

        IdentityResult result;
        if (lockAccount)
        {
            result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
        }
        else
        {
            result = await _userManager.SetLockoutEndDateAsync(user, null);
        }

        if (result.Succeeded)
        {
            string status = lockAccount ? "khóa" : "mở khóa";
            _logger.LogInformation("Toggled lockout status for User: ID={Id}, Email={Email}, Locked={Locked}", user.Id, user.Email, lockAccount);
            return OperationResult.SuccessResult($"Đã {status} tài khoản người dùng '{user.Email}' thành công.");
        }
        else
        {
            List<string> errors = result.Errors.Select(e => e.Description).ToList();
            _logger.LogError("Failed to toggle lockout for user '{Email}': {Errors}", user.Email, string.Join(", ", errors));
            return OperationResult.FailureResult(message: $"Không thể {(lockAccount ? "khóa" : "mở khóa")} tài khoản người dùng.", errors: errors);
        }
    }

    private async Task SyncUserRolesAsync(int userId, List<int> selectedRoleIds)
    {
        User? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return;

        var currentRoles = await _userManager.GetRolesAsync(user);
        var currentRoleIds = await _context.Set<Role>()
                                            .Where(r => currentRoles.Contains(r.Name ?? ""))
                                            .Select(r => r.Id)
                                            .ToListAsync();

        var rolesToRemove = currentRoles
            .Where(roleName => !_context.Set<Role>().Any(r => r.Name == roleName && selectedRoleIds.Contains(r.Id)))
            .ToList();

        var rolesToAddIds = selectedRoleIds
            .Where(selectedId => !currentRoleIds.Contains(selectedId))
            .ToList();

        if (rolesToRemove.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
        }

        if (rolesToAddIds.Any())
        {
            var roleNamesToAdd = await _context.Set<Role>()
                                               .Where(r => rolesToAddIds.Contains(r.Id))
                                               .Select(r => r.Name!)
                                               .ToListAsync();
            await _userManager.AddToRolesAsync(user, roleNamesToAdd);
        }
    }

    private async Task SyncUserClaimsAsync(int userId, List<int> selectedClaimDefinitionIds)
    {
        User? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return;

        var allClaimDefinitions = await _context.ClaimDefinitions
                                                .AsNoTracking()
                                                .ToDictionaryAsync(cd => cd.Id);

        var currentUserClaims = await _userManager.GetClaimsAsync(user);

        var claimsToRemove = new List<Claim>();
        foreach (var currentClaim in currentUserClaims)
        {
            if (currentClaim.Type == "Permission")
            {
                var matchingClaimDef = allClaimDefinitions.Values
                    .FirstOrDefault(cd => cd.Type == currentClaim.Type && cd.Value == currentClaim.Value);

                if (matchingClaimDef == null || !selectedClaimDefinitionIds.Contains(matchingClaimDef.Id))
                {
                    claimsToRemove.Add(currentClaim);
                }
            }
        }

        var claimsToAdd = new List<Claim>();
        foreach (var selectedId in selectedClaimDefinitionIds)
        {
            if (allClaimDefinitions.TryGetValue(selectedId, out var claimDef))
            {
                if (!currentUserClaims.Any(c => c.Type == claimDef.Type && c.Value == claimDef.Value))
                {
                    claimsToAdd.Add(new Claim(claimDef.Type, claimDef.Value));
                }
            }
            else
            {
                _logger.LogWarning("ClaimDefinition ID {ClaimId} not found when trying to add to User {UserId}.", selectedId, userId);
            }
        }

        foreach (var claim in claimsToRemove)
        {
            var removeResult = await _userManager.RemoveClaimAsync(user, claim);
            if (!removeResult.Succeeded)
            {
                _logger.LogError("Failed to remove claim '{ClaimType}:{ClaimValue}' from user '{UserName}': {Errors}", claim.Type, claim.Value, user.UserName, string.Join(", ", removeResult.Errors.Select(e => e.Description)));
            }
        }

        foreach (var claim in claimsToAdd)
        {
            var addResult = await _userManager.AddClaimAsync(user, claim);
            if (!addResult.Succeeded)
            {
                _logger.LogError("Failed to add claim '{ClaimType}:{ClaimValue}' to user '{UserName}': {Errors}", claim.Type, claim.Value, user.UserName, string.Join(", ", addResult.Errors.Select(e => e.Description)));
            }
        }
    }
}
