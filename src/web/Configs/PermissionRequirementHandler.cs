using Microsoft.AspNetCore.Authorization;

namespace web.Configs;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        // Kiểm tra xem người dùng có Claim nào có Type là "permission" (hoặc bất kỳ loại claim nào bạn dùng cho permission)
        // và Value khớp với requirement.Permission không.
        // Bạn cần đảm bảo ClaimDefinition của bạn có Type và Value phù hợp.
        // Giả sử Type của Claim Definition là "permission"
        // Hoặc bạn có thể định nghĩa Type là hằng số hoặc lấy từ cấu hình.
        // Ví dụ: Claim Type "Permission" và Claim Value "FAQ.View"
        // Identity RoleClaim và UserClaim sẽ lưu ClaimType và ClaimValue

        var hasPermission = context.User.HasClaim(claim =>
            claim.Type == "Permission" &&
            claim.Value == requirement.Permission);

        if (hasPermission)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail(new AuthorizationFailureReason(this, $"User does not have '{requirement.Permission}' permission."));
        }

        return Task.CompletedTask;
    }
}
