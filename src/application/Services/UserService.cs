using application.Interfaces;
using domain.Entities;
using infrastructure; // Namespace của ApplicationDbContext
using Microsoft.EntityFrameworkCore;
using shared.Constants;
using shared.Models;
using BC = BCrypt.Net.BCrypt;

namespace application.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context; // Inject DbContext

    public UserService(ApplicationDbContext context) // Constructor
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        try
        {
            // Direct query, including Role
            var users = await _context.Users
                .Include(u => u.Role)
                .ToListAsync();

            return users;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        try
        {
            // Direct query
            return await _context.Users
                .Where(u => u.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<BaseResponse> AddAsync(User user)
    {
        try
        {
            // Validation and direct add
            var existingUsers = await _context.Users
                .Where(u => u.Username == user.Username || u.Email == user.Email)
                .ToListAsync();

            var errors = new Dictionary<string, string[]>();

            if (existingUsers.Any(u => u.Username == user.Username))
                errors.Add(nameof(user.Username), ["Tên người dùng đã tồn tại."]);

            if (existingUsers.Any(u => u.Email == user.Email)) errors.Add(nameof(user.Email), ["Email đã tồn tại."]);

            if (errors.Count != 0) return new ErrorResponse(errors);

            // Find the default role directly
            var defaultRole = await _context.Roles
                                  .Where(r => r.Name == RoleConstants.User)
                                  .FirstOrDefaultAsync() ??
                              throw new Exception("Role mặc định không tồn tại. Vui lòng chạy seeding cho role.");
            user.PasswordHash = BC.HashPassword(user.PasswordHash); // Hash the password
            user.RoleId = defaultRole.Id;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return new SuccessResponse<User>(user, "Đăng ký thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            });
        }
    }

    public async Task<BaseResponse> UpdateAsync(int id, User user)
    {
        try
        {
            // Find existing user
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (existingUser == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Người dùng không tồn tại"] }
                });
            //Check Duplicate
            var duplicateUser = await _context.Users.FirstOrDefaultAsync(u => u.Id != id && (u.Username == user.Username || u.Email == user.Email));
            var errors = new Dictionary<string, string[]>();
            if (duplicateUser != null)
            {
                if (duplicateUser.Username == user.Username)
                    errors.Add(nameof(user.Username), ["Tên người dùng đã tồn tại."]);

                if (duplicateUser.Email == user.Email) errors.Add(nameof(user.Email), ["Email đã tồn tại."]);

                if (errors.Count != 0) return new ErrorResponse(errors);
            }
            // Update fields.  Make sure to update ALL relevant fields.
            existingUser.RoleId = user.RoleId;
            existingUser.Username = user.Username ?? existingUser.Username;  // Important!
            existingUser.Email = user.Email ?? existingUser.Email;    // Important!
                                                                      // ... other fields you want to update (e.g., FullName, etc.) ...

            // Update password only if a new password is provided
            if (!string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                existingUser.PasswordHash = BC.HashPassword(user.PasswordHash);
            }
            await _context.SaveChangesAsync();

            return new SuccessResponse<User>(existingUser, "Cập nhật thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            });
        }
    }
}