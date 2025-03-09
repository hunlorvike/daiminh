using Microsoft.AspNetCore.Http;

namespace shared.Extensions;

public static class HttpRequestExtensions
{
    public static bool IsAjaxRequest(this HttpRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return request.Headers.XRequestedWith == "XMLHttpRequest";
    }
}