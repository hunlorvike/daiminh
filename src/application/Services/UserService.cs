using application.Interfaces;
using domain.Entities;
using infrastructure; // Namespace của ApplicationDbContext
using Microsoft.EntityFrameworkCore;
using shared.Constants;
using shared.Models;
using BC = BCrypt.Net.BCrypt;

namespace application.Services;

/// <summary>
/// Service for managing users.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="UserService"/> class.
/// </remarks>
/// <param name="context">The application database context.</param>
public class UserService(ApplicationDbContext context) : IUserService
{

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>A list of users.</returns>
    public async Task<List<User>> GetAllAsync()
    {
        try
        {
            // Retrieve all users from the database, including their roles.
            var users = await context.Users
                .Include(u => u.Role)
                .Where(x => x.DeletedAt == null)
                .AsNoTracking()
                .ToListAsync();

            return users;
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, "GetAllAsync UserService");
            throw new Exception("Đã xảy ra lỗi khi lấy danh sách người dùng.", ex);
        }
    }

    /// <summary>
    /// Retrieves a user by its ID.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The user, or null if not found.</returns>
    public async Task<User?> GetByIdAsync(int id)
    {
        try
        {
            // Retrieve a user by ID from the database.
            return await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null); // Use FirstOrDefaultAsync
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"GetByIdAsync UserService with id: {id}");
            throw new Exception($"Đã xảy ra lỗi khi lấy thông tin người dùng có ID: {id}.", ex);
        }
    }

    /// <summary>
    /// Adds a new user.
    /// </summary>
    /// <param name="user">The user to add.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> AddAsync(User user)
    {
        try
        {
            // Check for existing users with the same username or email.
            var existingUsers = await context.Users
                .Where(u => (u.Username == user.Username || u.Email == user.Email) && u.DeletedAt == null)
                .ToListAsync();

            var errors = new Dictionary<string, string[]>();

            if (existingUsers.Any(u => u.Username == user.Username))
                errors.Add(nameof(user.Username), ["Tên đăng nhập này đã được sử dụng. Vui lòng chọn một tên khác."]);

            if (existingUsers.Any(u => u.Email == user.Email)) errors.Add(nameof(user.Email), ["Địa chỉ email này đã được đăng ký. Vui lòng sử dụng một địa chỉ email khác."]);

            if (errors.Count != 0) return new ErrorResponse(errors);

            // Find the default user role.
            var defaultRole = await context.Roles
                                  .Where(r => r.Name == RoleConstants.User)
                                  .FirstOrDefaultAsync() ??
                              throw new Exception("Không tìm thấy role mặc định. Vui lòng kiểm tra lại cấu hình hệ thống.");

            // Hash the user's password.
            user.PasswordHash = BC.HashPassword(user.PasswordHash);
            user.RoleId = defaultRole.Id;

            // Add the new user to the database.
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return new SuccessResponse<User>(user, "Thêm người dùng mới thành công.");
        }
        catch (Exception)
        {
            //log
            //_logger.LogError(ex, "AddAsync UserService");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi thêm người dùng mới. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="user">The updated user data.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> UpdateAsync(int id, User user)
    {
        try
        {
            // Find the existing user by ID.
            var existingUser = await context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null);

            if (existingUser == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Người dùng không tồn tại hoặc đã bị xóa."] }
                });

            //Check Duplicate
            var duplicateUser = await context.Users.FirstOrDefaultAsync(u => u.Id != id && (u.Username == user.Username || u.Email == user.Email) && u.DeletedAt == null);
            var errors = new Dictionary<string, string[]>();
            if (duplicateUser != null)
            {
                if (duplicateUser.Username == user.Username)
                    errors.Add(nameof(user.Username), ["Tên người dùng đã tồn tại."]);

                if (duplicateUser.Email == user.Email) errors.Add(nameof(user.Email), ["Email đã tồn tại."]);

                if (errors.Count != 0) return new ErrorResponse(errors);
            }

            // Update the user properties.
            existingUser.RoleId = user.RoleId;
            existingUser.Username = user.Username ?? existingUser.Username;  // Important!
            existingUser.Email = user.Email ?? existingUser.Email;    // Important!
                                                                      // ... other fields you want to update (e.g., FullName, etc.) ...

            // Update password only if a new password is provided.
            if (!string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                existingUser.PasswordHash = BC.HashPassword(user.PasswordHash);
            }
            await context.SaveChangesAsync();

            return new SuccessResponse<User>(existingUser, "Cập nhật thông tin người dùng thành công.");
        }
        catch (Exception)
        {
            //log
            //_logger.LogError(ex, $"UpdateAsync UserService with id: {id}");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi cập nhật thông tin người dùng. Vui lòng thử lại sau."] }
            });
        }
    }
    /// <summary>
    /// Deletes a user (soft delete).
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> DeleteAsync(int id)
    {
        try
        {
            // Find the user by ID (only if not already soft-deleted).
            var user = await context.Users.FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null);

            if (user == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["user không tồn tại hoặc đã bị xóa."] } });

            // Perform a soft delete by setting the DeletedAt property.
            user.DeletedAt = DateTime.UtcNow; // Soft delete

            await context.SaveChangesAsync();

            return new SuccessResponse<User>(user, "Xóa user thành công (đã ẩn).");
        }
        catch (Exception)
        {
            //log
            //_logger.LogError(ex, $"DeleteAsync UserService with id: {id}");
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", ["Đã xảy ra lỗi khi xóa user. Vui lòng thử lại sau."] } });
        }
    }
}