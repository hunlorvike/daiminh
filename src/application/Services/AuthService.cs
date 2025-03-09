using System.Security.Claims;
using application.Interfaces;
using domain.Constants;
using domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using shared.Attributes;
using shared.Interfaces;
using shared.Models;
using BC = BCrypt.Net.BCrypt;

namespace application.Services;

public class AuthService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : ScopedService, IAuthService
{
    public async Task<BaseResponse> SignUpAsync(User user)
    {
        try
        {
            var userRepository = unitOfWork.GetRepository<User, int>();
            var roleRepository = unitOfWork.GetRepository<Role, int>();

            var existingUsers = await userRepository
                .Where(u => u.Username == user.Username || u.Email == user.Email)
                .ToListAsync();

            var errors = new Dictionary<string, string[]>();

            if (existingUsers.Any(u => u.Username == user.Username))
                errors.Add(nameof(user.Username), ["Tên người dùng đã tồn tại."]);

            if (existingUsers.Any(u => u.Email == user.Email)) errors.Add(nameof(user.Email), ["Email đã tồn tại."]);

            if (errors.Count != 0) return new ErrorResponse(errors);

            var defaultRole = await roleRepository
                                  .Where(r => r.Name == RoleConstants.User)
                                  .FirstOrDefaultAsync() ??
                              throw new Exception("Role mặc định không tồn tại. Vui lòng chạy seeding cho role.");
            user.PasswordHash = BC.HashPassword(user.PasswordHash);
            user.RoleId = defaultRole.Id;

            await userRepository.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

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

    public async Task<BaseResponse> SignInAsync(User user, string scheme)
    {
        try
        {
            var userRepository = unitOfWork.GetRepository<User, int>();

            var existingUser = await userRepository
                .Include(r => r.Role)
                .Where(u => u.Username == user.Username)
                .FirstOrDefaultAsync();

            if (existingUser == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(user.Username), ["Người dùng không tồn tại."] }
                });

            var isValidPassword = BC.Verify(user.PasswordHash, existingUser.PasswordHash);
            if (!isValidPassword)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "Password", ["Mật khẩu không đúng."] }
                });

            List<Claim> claims =
            [
                new(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                new(ClaimTypes.Email, existingUser.Email),
                new(ClaimTypes.Role, existingUser.Role?.Name ?? string.Empty)
            ];

            var claimsIdentity = new ClaimsIdentity(claims, scheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
            };

            await httpContextAccessor.HttpContext!.SignInAsync(
                scheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return new SuccessResponse<User>(existingUser, "Đăng nhập thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            });
        }
    }

    public async Task SignOutAsync(string scheme)
    {
        try
        {
            await httpContextAccessor.HttpContext!.SignOutAsync(scheme);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}