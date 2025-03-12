using Microsoft.AspNetCore.Http;

namespace shared.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="HttpRequest"/> class.
/// </summary>
public static class HttpRequestExtensions
{
    /// <summary>
    /// Determines whether the specified HTTP request is an AJAX request.
    /// </summary>
    /// <param name="request">The HTTP request.</param>
    /// <returns>
    ///   <c>true</c> if the specified HTTP request is an AJAX request; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="request"/> is null.</exception>
    public static bool IsAjaxRequest(this HttpRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return request.Headers.XRequestedWith == "XMLHttpRequest";
    }
}