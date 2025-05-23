using Microsoft.AspNetCore.Authorization;

namespace web.Configs;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; } // Ví dụ: "FAQ.View", "Product.Create"

    public PermissionRequirement(string permission)
    {
        Permission = permission ?? throw new ArgumentNullException(nameof(permission));
    }
}
