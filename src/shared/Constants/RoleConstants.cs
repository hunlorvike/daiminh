namespace shared.Constants;

/// <summary>
/// Defines constants for common user roles within the application.
/// Using constants for roles improves maintainability, reduces the risk of typos, and centralizes role definitions.
/// </summary>
/// <remarks>
/// These constants can be used throughout the application when checking user roles,
/// for example, with the `[Authorize(Roles = RoleConstants.Admin)]` attribute.
/// </remarks>
public static class RoleConstants
{
    /// <summary>
    /// Represents the administrator role.  Administrators typically have full access to all features and data.
    /// </summary>
    public const string Admin = "Admin";

    /// <summary>
    /// Represents the standard user role.  Standard users have basic access to the application.
    /// </summary>
    public const string User = "User";

    /// <summary>
    /// Represents the manager role. Managers may have elevated permissions compared to standard users,
    /// but less than administrators.
    /// </summary>
    public const string Manager = "Manager";
}