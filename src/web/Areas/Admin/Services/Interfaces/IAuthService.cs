using shared.Models;

namespace web.Areas.Admin.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResult> AuthenticateAdminUserAsync(string username, string password);
}
