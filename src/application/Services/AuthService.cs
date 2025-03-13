using application.Interfaces;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;
using shared.Constants;
using shared.Models;
using System.Security.Claims;
using BC = BCrypt.Net.BCrypt;

namespace application.Services;

/// <summary>
/// Service for handling authentication-related operations.
/// </summary>
public class AuthService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : IAuthService
{
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="user">The user object containing registration details.</param>
    /// <returns>A BaseResponse indicating success or failure, with user data on success or error messages on failure.</returns>
    /// <exception cref="Exception">Thrown if the default user role is not found in the database.</exception>
    public async Task<BaseResponse> SignUpAsync(User user)
    {
        try
        {
            // Check for existing users with the same username or email.
            var existingUsers = await context.Users
                .Where(u => u.Username == user.Username || u.Email == user.Email)
                .ToListAsync();

            var errors = new Dictionary<string, string[]>();

            // Add error messages if duplicates are found.
            if (existingUsers.Any(u => u.Username == user.Username))
                errors.Add(nameof(user.Username), ["Tên đăng nhập này đã được sử dụng. Vui lòng chọn một tên khác."]);

            if (existingUsers.Any(u => u.Email == user.Email))
                errors.Add(nameof(user.Email), ["Địa chỉ email này đã được đăng ký. Vui lòng sử dụng một địa chỉ email khác."]);

            // Return an error response if any duplicates were found.
            if (errors.Count != 0) return new ErrorResponse(errors);

            // Retrieve the default user role, or throw an exception if it's not found.
            var defaultRole = await context.Roles
                                  .Where(r => r.Name == RoleConstants.User)
                                  .FirstOrDefaultAsync() ??
                              throw new Exception("Không tìm thấy role mặc định. Vui lòng kiểm tra lại cấu hình hệ thống.");

            // Hash the user's password for security.
            user.PasswordHash = BC.HashPassword(user.PasswordHash);
            user.RoleId = defaultRole.Id;

            // Add the user to the database and save changes.
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            // Return a success response with the newly created user data.
            return new SuccessResponse<User>(user, "Đăng ký tài khoản thành công.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during user registration.");
            // Return a generic error response to the user.
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi trong quá trình đăng ký. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Signs in a user.
    /// </summary>
    /// <param name="user">The user object containing sign-in credentials.</param>
    /// <param name="scheme">The authentication scheme to use.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> SignInAsync(User user, string scheme)
    {
        try
        {
            // Find the user by username, including their role.
            var existingUser = await context.Users
                .Include(u => u.Role)
                .Where(u => u.Username == user.Username)
                .FirstOrDefaultAsync();

            // Return an error if the user is not found.
            if (existingUser == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(user.Username), ["Tên đăng nhập không tồn tại."] }
                });

            // Verify the provided password against the stored hashed password.
            var isValidPassword = BC.Verify(user.PasswordHash, existingUser.PasswordHash);
            if (!isValidPassword)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "Password", ["Mật khẩu không chính xác."] }
                });

            // Create claims for the authenticated user.
            List<Claim> claims =
            [
                new(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                new(ClaimTypes.Email, existingUser.Email),
                new(ClaimTypes.Role, existingUser.Role?.Name ?? string.Empty) // Use null-conditional operator for safety.
            ];

            // Create a ClaimsIdentity and AuthenticationProperties.
            var claimsIdentity = new ClaimsIdentity(claims, scheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Remember the user's login.
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24) // Set an expiration time for the login session.
            };

            // Sign in the user using the specified authentication scheme.
            await httpContextAccessor.HttpContext!.SignInAsync(
                scheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return new SuccessResponse<User>(existingUser, "Đăng nhập thành công.");
        }
        catch (Exception ex)
        {
            // Log the exception.
            Log.Error(ex, "Error during user sign-in.");

            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi trong quá trình đăng nhập. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Signs out the current user.
    /// </summary>
    /// <param name="scheme">The authentication scheme to use.</param>
    /// <returns>A Task representing the asynchronous sign-out operation.</returns>
    /// <exception cref="Exception">Rethrows any exception that occurs during sign-out.</exception>
    public async Task SignOutAsync(string scheme)
    {
        try
        {
            // Sign out the user using the specified authentication scheme.
            await httpContextAccessor.HttpContext!.SignOutAsync(scheme);
        }
        catch (Exception ex)
        {
            // Log the exception
            Log.Error(ex, "Error during user sign-out.");

            // Rethrow to handle the exception
            throw new Exception("Đã xảy ra lỗi khi đăng xuất.", ex);
        }
    }
}