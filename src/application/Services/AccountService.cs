using application.Dtos.Account;
using application.Services.Interfaces;
using domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using shared.Models;
using System.Security.Claims;

namespace application.Services;
public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AccountService> _logger;
    private readonly RoleManager<Role> _roleManager;

    public AccountService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    ILogger<AccountService> logger,
    RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _roleManager = roleManager;
    }

    public async Task<Result<List<Claim>>> LoginAsync(LoginDto loginDto)
    {
        User? user = await _userManager.FindByNameAsync(loginDto.Username);

        if (user == null)
        {
            _logger.LogWarning("Login failed - user not found: {Username}", loginDto.Username);
            return Result<List<Claim>>.Failure("Auth.InvalidCredentials", "Tên đăng nhập hoặc mật khẩu không chính xác.");
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("Login failed - account inactive: {Username}", user.UserName);
            return Result<List<Claim>>.Failure("Auth.AccountInactive", "Tài khoản của bạn đã bị khóa hoặc chưa được kích hoạt.");
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

        if (signInResult.Succeeded)
        {
            _logger.LogInformation("Authentication successful for user: {Username}", user.UserName);
            var claims = await _GetUserClaimsAsync(user);
            return Result<List<Claim>>.Success(claims);
        }

        if (signInResult.IsLockedOut)
        {
            _logger.LogWarning("Authentication failed - account locked out: {Username}", user.UserName);
            return Result<List<Claim>>.Failure("Auth.LockedOut", "Tài khoản của bạn đã bị khóa. Vui lòng thử lại sau.");
        }

        _logger.LogWarning("Authentication failed - incorrect password for user: {Username}", user.UserName);
        return Result<List<Claim>>.Failure("Auth.InvalidCredentials", "Tên đăng nhập hoặc mật khẩu không chính xác.");
    }

    private async Task<List<Claim>> _GetUserClaimsAsync(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName!),
        };

        if (!string.IsNullOrEmpty(user.Email))
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

        if (!string.IsNullOrEmpty(user.FullName))
            claims.Add(new Claim(ClaimTypes.GivenName, user.FullName));

        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var roleName in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                claims.AddRange(roleClaims);
            }
        }

        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        return claims.DistinctBy(c => new { c.Type, c.Value }).ToList();
    }
}
