using System.Security.Claims;
using core.Entities;
using core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace core.Services;

public class AuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task SignUpAsync(User user)
    {
        try
        {
            var userRepository = _unitOfWork.GetRepository<User, int>();
            var roleRepository = _unitOfWork.GetRepository<Role, int>();

            var existingUsers = await userRepository.Where(u => u.Username == user.Username || u.Email == user.Email)
                .FirstOrDefaultAsync();

            if (existingUsers != null) throw new Exception("Tên người dùng hoặc email đã tồn tại.");

            // TODO: sửa lại thành user role
            var defaultRole = await roleRepository.Where(r => r.Name == RoleConstants.Admin).FirstOrDefaultAsync();

            if (defaultRole == null) throw new Exception("Chạy seeding cho role");

            user.PasswordHash = BC.HashPassword(user.PasswordHash);
            user.RoleId = defaultRole.Id;

            await userRepository.AddAsync(user);

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // TODO: Log the exception (ex) here if needed
            throw new Exception(ex.Message);
        }
    }

    public async Task SignInAsync(User user, string scheme = "AdminAuth")
    {
        try
        {
            var userRepository = _unitOfWork.GetRepository<User, int>();

            var existingUsers = await userRepository
                .Where(u => u.Username == user.Username)
                .FirstOrDefaultAsync();

            if (existingUsers == null) throw new Exception("Người dùng không tồn tại.");

            var isValidPassword = BC.Verify(user.PasswordHash, existingUsers.PasswordHash);
            if (!isValidPassword) throw new Exception("Mật khẩu không đúng.");

            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, existingUsers.Email),
                new(ClaimTypes.NameIdentifier, existingUsers.Id.ToString()),
                new(ClaimTypes.Role, existingUsers.Role?.Name ?? string.Empty)
            };

            var claimsIdentity = new ClaimsIdentity(claims, scheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
            };

            await _httpContextAccessor.HttpContext!.SignInAsync(
                scheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
        catch (Exception ex)
        {
            // TODO: Log the exception (ex) here if needed
            throw new Exception(ex.Message);
        }
    }

    public async Task SignOutAsync(string scheme = "AdminAuth")
    {
        try
        {
            await _httpContextAccessor.HttpContext!.SignOutAsync(scheme);
        }
        catch (Exception ex)
        {
            // TODO: Log the exception (ex) here if needed
            throw new Exception(ex.Message);
        }
    }
}