namespace shared.Constants;

/// <summary>
/// Defines constants for cookie authentication schema names.  These constants are used to
/// identify different authentication schemes when using cookie-based authentication in ASP.NET Core.
/// </summary>
/// <remarks>
/// Using constants for schema names improves maintainability and reduces the risk of typos.
/// The schema names are prefixed with ".AspNetCore.Cookie." which is a common convention.  The
/// distinct suffixes ("AdminSchema" and "UserSchema") differentiate between administrator and regular user
/// authentication contexts.
/// </remarks>
public static class CookiesConstants
{
    /// <summary>
    /// The authentication schema name for administrator users.  This schema is used when
    /// authenticating and authorizing administrators.
    /// </summary>
    public const string AdminCookieSchema = $".AspNetCore.Cookie.AdminSchema";

    /// <summary>
    /// The authentication schema name for regular users.  This schema is used when
    /// authenticating and authorizing non-administrator users.
    /// </summary>
    public const string UserCookieSchema = $".AspNetCore.Cookie.UserSchema";
}