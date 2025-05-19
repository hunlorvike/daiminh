using AutoMapper;
using AutoMapper.QueryableExtensions;
using infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly IPasswordHasher<domain.Entities.User> _passwordHasher;

    public UserService(ApplicationDbContext context, IMapper mapper, ILogger<UserService> logger, IPasswordHasher<domain.Entities.User> passwordHasher)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    public async Task<IPagedList<UserListItemViewModel>> GetPagedUsersAsync(UserFilterViewModel filter, int pageNumber, int pageSize)
    {
        IQueryable<domain.Entities.User> query = _context.Set<domain.Entities.User>().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(u => u.UserName.ToLower().Contains(lowerSearchTerm) ||
                                     u.Email.ToLower().Contains(lowerSearchTerm) ||
                                     u.FullName != null && u.FullName.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(u => u.IsActive == filter.IsActive.Value);
        }

        query = query.OrderBy(u => u.UserName);

        IPagedList<UserListItemViewModel> usersPaged = await query
            .ProjectTo<UserListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        return usersPaged;
    }

    public async Task<UserEditViewModel?> GetUserByIdAsync(int id)
    {
        domain.Entities.User? user = await _context.Set<domain.Entities.User>()
                               .AsNoTracking()
                               .FirstOrDefaultAsync(u => u.Id == id);

        return _mapper.Map<UserEditViewModel>(user);
    }

    public async Task<OperationResult<int>> CreateUserAsync(UserCreateViewModel viewModel)
    {
        if (await IsUsernameUniqueAsync(viewModel.UserName))
        {
            return OperationResult<int>.FailureResult(message: "Tên đăng nhập đã tồn tại.", errors: new List<string> { "Tên đăng nhập đã tồn tại." });
        }
        if (!string.IsNullOrEmpty(viewModel.Email) && await IsEmailUniqueAsync(viewModel.Email))
        {
            return OperationResult<int>.FailureResult(message: "Email đã tồn tại.", errors: new List<string> { "Email đã tồn tại." });
        }

        var user = _mapper.Map<domain.Entities.User>(viewModel);

        if (!string.IsNullOrEmpty(viewModel.Password))
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, viewModel.Password);
        }

        _context.Add(user);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created User: ID={Id}, Username={Username}", user.Id, user.UserName);
            return OperationResult<int>.SuccessResult(user.Id, $"Thêm người dùng '{user.UserName}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi tạo người dùng: {Username}", viewModel.UserName);
            if (ex.InnerException?.Message?.Contains("UQ_User_Username", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult<int>.FailureResult(message: "Tên đăng nhập đã tồn tại.", errors: new List<string> { "Tên đăng nhập đã tồn tại." });
            }
            if (ex.InnerException?.Message?.Contains("UQ_User_Email", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult<int>.FailureResult(message: "Email đã tồn tại.", errors: new List<string> { "Email đã tồn tại." });
            }
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi hệ thống khi tạo người dùng.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi tạo người dùng." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi tạo người dùng: {Username}", viewModel.UserName);
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi hệ thống khi tạo người dùng.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi tạo người dùng." });
        }
    }

    public async Task<OperationResult> UpdateUserAsync(UserEditViewModel viewModel, int currentUserId)
    {
        if (await IsUsernameUniqueAsync(viewModel.UserName, viewModel.Id))
        {
            return OperationResult.FailureResult(message: "Tên đăng nhập đã tồn tại.", errors: new List<string> { "Tên đăng nhập đã tồn tại." });
        }
        if (!string.IsNullOrEmpty(viewModel.Email) && await IsEmailUniqueAsync(viewModel.Email, viewModel.Id))
        {
            return OperationResult.FailureResult(message: "Email đã tồn tại.", errors: new List<string> { "Email đã tồn tại." });
        }

        var user = await _context.Set<domain.Entities.User>().FirstOrDefaultAsync(u => u.Id == viewModel.Id);
        if (user == null)
        {
            _logger.LogWarning("User not found for update. ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult("Không tìm thấy người dùng để cập nhật.");
        }

        if (!viewModel.IsActive && (user.Id == currentUserId || user.Id == 1))
        {
            _logger.LogWarning("Attempt to deactivate self ({CurrentUserId}) or primary admin (ID=1)", currentUserId);
            return OperationResult.FailureResult(message: "Không thể hủy kích hoạt chính mình hoặc tài khoản quản trị viên chính.", errors: new List<string> { "Không thể hủy kích hoạt chính mình hoặc tài khoản quản trị viên chính." });
        }

        string? originalPasswordHash = user.PasswordHash;

        _mapper.Map(viewModel, user);

        user.PasswordHash = originalPasswordHash;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated User: ID={Id}, Username={Username}, IsActive={IsActive}", user.Id, user.UserName, user.IsActive);
            return OperationResult.SuccessResult($"Cập nhật người dùng '{user.UserName}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật người dùng ID: {Id}, Username {Username}", viewModel.Id, viewModel.UserName);
            if (ex.InnerException?.Message?.Contains("UQ_User_Username", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult(message: "Tên đăng nhập đã tồn tại.", errors: new List<string> { "Tên đăng nhập đã tồn tại." });
            }
            if (ex.InnerException?.Message?.Contains("UQ_User_Email", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult(message: "Email đã tồn tại.", errors: new List<string> { "Email đã tồn tại." });
            }
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi hệ thống khi cập nhật người dùng.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi cập nhật người dùng." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật người dùng ID: {Id}, Username {Username}", viewModel.Id, viewModel.UserName);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi hệ thống khi cập nhật người dùng.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi cập nhật người dùng." });
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

        domain.Entities.User? user = await _context.Set<domain.Entities.User>().FindAsync(userId);

        if (user == null)
        {
            _logger.LogWarning("User not found for delete. ID: {Id}", userId);
            return OperationResult.FailureResult("Không tìm thấy người dùng.");
        }

        string username = user.UserName;

        _context.Remove(user);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted User: ID={Id}, Username={Username}", userId, username);
            return OperationResult.SuccessResult($"Xóa người dùng '{username}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi xóa người dùng ID {Id}", userId);
            if (ex.InnerException?.Message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult($"Không thể xóa người dùng '{username}' vì đang được sử dụng.", errors: new List<string> { $"Không thể xóa người dùng '{username}' vì đang được sử dụng." });
            }
            return OperationResult.FailureResult("Lỗi cơ sở dữ liệu khi xóa người dùng.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi xóa người dùng." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi xóa người dùng ID {Id}", userId);
            return OperationResult.FailureResult($"Không thể xóa người dùng '{username}'.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi xóa người dùng." });
        }
    }

    public async Task<bool> IsUsernameUniqueAsync(string username, int? ignoreId = null)
    {
        if (string.IsNullOrWhiteSpace(username)) return false;

        var lowerUsername = username.Trim().ToLower();
        var query = _context.Set<domain.Entities.User>()
                            .Where(u => u.UserName.ToLower() == lowerUsername);

        if (ignoreId.HasValue && ignoreId.Value > 0)
        {
            query = query.Where(u => u.Id != ignoreId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<bool> IsEmailUniqueAsync(string email, int? ignoreId = null)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        var lowerEmail = email.Trim().ToLower();
        var query = _context.Set<domain.Entities.User>()
                            .Where(u => !string.IsNullOrEmpty(u.Email) && u.Email.ToLower() == lowerEmail);

        if (ignoreId.HasValue && ignoreId.Value > 0)
        {
            query = query.Where(u => u.Id != ignoreId.Value);
        }

        return await query.AnyAsync();
    }
}