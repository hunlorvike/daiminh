using Microsoft.AspNetCore.Authorization;

namespace web.Configs;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly ILogger<PermissionRequirementHandler> _logger;

    public PermissionRequirementHandler(ILogger<PermissionRequirementHandler> logger)
    {
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User == null || !context.User.Identity?.IsAuthenticated == true)
        {
            _logger.LogWarning("Authorization failed for permission '{Permission}': User is not authenticated.", requirement.Permission);
            context.Fail();
            return Task.CompletedTask;
        }

        if (context.User.HasClaim(claim => claim.Type == "Permission" && claim.Value == "SuperAdmin.Access"))
        {
            _logger.LogInformation("Authorization succeeded for permission '{Permission}': User '{UserName}' is SuperAdmin.",
                                   requirement.Permission, context.User.Identity.Name);
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var hasSpecificPermission = context.User.HasClaim(claim =>
            claim.Type == "Permission" &&
            claim.Value == requirement.Permission);

        if (hasSpecificPermission)
        {
            _logger.LogInformation("Authorization succeeded for permission '{Permission}': User '{UserName}' has the required claim.",
                                   requirement.Permission, context.User.Identity.Name);
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogWarning("Authorization failed for permission '{Permission}': User '{UserName}' does not have the required claim.",
                               requirement.Permission, context.User.Identity.Name);
            context.Fail(new AuthorizationFailureReason(this, $"Người dùng không có quyền '{requirement.Permission}' để truy cập tài nguyên này."));
        }

        return Task.CompletedTask;
    }
}