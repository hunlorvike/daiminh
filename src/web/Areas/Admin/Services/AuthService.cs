using AutoRegister;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using shared.Models;
using System.Security.Claims;
using web.Areas.Admin.Services.Interfaces;

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AuthService> _logger;
    private readonly ApplicationDbContext _dbContext;

    public AuthService(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        SignInManager<User> signInManager,
        ILogger<AuthService> logger,
        ApplicationDbContext dbContext)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<LoginResult> AuthenticateAdminUserAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            _logger.LogWarning("Authentication failed - user not found: {Username}", username);
            return LoginResult.Failure("Tên đăng nhập hoặc mật khẩu không chính xác.");
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("Authentication failed - account inactive: {Username}", user.UserName);
            return LoginResult.Failure("Tài khoản của bạn chưa được kích hoạt hoặc đã bị khóa.");
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);

        if (signInResult.Succeeded)
        {
            _logger.LogInformation("Authentication successful for user: {Username}", user.UserName);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? user.Email!),
            };

            if (!string.IsNullOrEmpty(user.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
            }

            var allClaimDefinitions = await _dbContext.ClaimDefinitions
                                                      .AsNoTracking()
                                                      .ToDictionaryAsync(cd => cd.Value);

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var roleName in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));

                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (var roleClaim in roleClaims)
                    {
                        if (roleClaim.Type == "Permission" && allClaimDefinitions.ContainsKey(roleClaim.Value))
                        {
                            claims.Add(roleClaim);
                        }
                    }
                }
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            foreach (var userClaim in userClaims)
            {
                if (userClaim.Type == "Permission" && allClaimDefinitions.ContainsKey(userClaim.Value))
                {
                    claims.Add(userClaim);
                }
            }

            claims = claims.GroupBy(c => new { c.Type, c.Value }).Select(g => g.First()).ToList();

            bool requiresRehash = false;
            if (user.PasswordHash != null)
            {
                var passwordVerificationResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                if (passwordVerificationResult == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    requiresRehash = true;
                    _logger.LogInformation("Password requires rehash for user: {Username}", user.UserName);
                }
            }

            return LoginResult.Succeed(claims, requiresRehash, "Đăng nhập thành công!");
        }

        if (signInResult.IsLockedOut)
        {
            _logger.LogWarning("Authentication failed - account locked out: {Username}", user.UserName);
            var lockoutEndDate = await _userManager.GetLockoutEndDateAsync(user);
            var remainingLockoutTime = lockoutEndDate.HasValue ? lockoutEndDate.Value - DateTimeOffset.UtcNow : TimeSpan.Zero;
            string message = $"Tài khoản của bạn đã bị khóa. Vui lòng thử lại sau {Math.Ceiling(remainingLockoutTime.TotalMinutes)} phút.";
            if (remainingLockoutTime.TotalSeconds <= 0)
            {
                message = "Tài khoản của bạn đã bị khóa. Vui lòng thử lại sau ít phút.";
            }
            return LoginResult.Failure(message);
        }

        if (signInResult.IsNotAllowed)
        {
            _logger.LogWarning("Authentication failed - account not allowed to sign in (e.g., email not confirmed): {Username}", user.UserName);
            return LoginResult.Failure("Tài khoản của bạn không được phép đăng nhập. Vui lòng xác thực tài khoản hoặc liên hệ quản trị viên.");
        }

        if (signInResult.RequiresTwoFactor)
        {
            _logger.LogWarning("Authentication failed - two-factor authentication required: {Username}", user.UserName);
            return LoginResult.Failure("Yêu cầu xác thực hai yếu tố.");
        }

        _logger.LogWarning("Authentication failed - incorrect password for user: {Username}", user.UserName);
        return LoginResult.Failure("Tên đăng nhập hoặc mật khẩu không chính xác.");
    }
}