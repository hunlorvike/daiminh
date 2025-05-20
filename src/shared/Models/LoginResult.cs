using System.Security.Claims;

namespace shared.Models;

public class LoginResult : OperationResult
{
    public List<Claim>? Claims { get; set; }
    public bool RequiresPasswordRehash { get; set; }

    public static LoginResult Succeed(List<Claim> claims, bool requiresRehash = false, string? message = null)
        => new LoginResult { Success = true, Claims = claims, RequiresPasswordRehash = requiresRehash, Message = message };

    public static LoginResult Failure(string? message = null, List<string>? errors = null)
        => new LoginResult { Success = false, Message = message, Errors = errors ?? new List<string>() };
}
