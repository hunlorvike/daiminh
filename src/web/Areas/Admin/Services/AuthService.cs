using infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using shared.Models;
using System.Security.Claims;
using web.Areas.Admin.Services.Interfaces;

namespace web.Areas.Admin.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<domain.Entities.User> _passwordHasher;
    private readonly ILogger<AuthService> _logger;

    public AuthService(ApplicationDbContext context, IPasswordHasher<domain.Entities.User> passwordHasher, ILogger<AuthService> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<LoginResult> AuthenticateAdminUserAsync(string username, string password)
    {
        var user = await _context.Set<domain.Entities.User>()
                                 .FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());

        if (user == null)
        {
            _logger.LogWarning("Authentication failed - user not found: {Username}", username);
            return LoginResult.Failure("Tên đăng nhập hoặc mật khẩu không chính xác.");
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("Authentication failed - account locked: {Username}", user.UserName);
            return LoginResult.Failure("Tài khoản của bạn đã bị khóa.");
        }

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            _logger.LogWarning("Authentication failed - incorrect password: {Username}", user.UserName);
            return LoginResult.Failure("Tên đăng nhập hoặc mật khẩu không chính xác.");
        }

        var roles = await (from ur in _context.UserRoles
                           join r in _context.Roles on ur.RoleId equals r.Id
                           where ur.UserId == user.Id
                           select r.Name).ToListAsync();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        bool requiresRehash = passwordVerificationResult == PasswordVerificationResult.SuccessRehashNeeded;
        if (requiresRehash)
        {
            try
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, password);
                _context.Update(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Password rehashed after login for: {Username}", user.UserName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not rehash password for user '{Username}'", user.UserName);
                requiresRehash = false;
            }
        }

        _logger.LogInformation("Authentication successful: {Username}", user.UserName);
        return LoginResult.Succeed(claims, requiresRehash, "Đăng nhập thành công!");
    }
}