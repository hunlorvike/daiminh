using web.Areas.Admin.Services;

namespace web.Middlewares;

public class RedirectMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RedirectMiddleware> _logger;

    public RedirectMiddleware(RequestDelegate next, ILogger<RedirectMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IRedirectService redirectService)
    {
        // Skip for non-GET requests or API endpoints
        if (context.Request.Method != "GET" || context.Request.Path.StartsWithSegments("/api"))
        {
            await _next(context);
            return;
        }

        // Check for redirect
        var path = context.Request.Path.Value ?? "/";
        var redirectResult = await redirectService.CheckForRedirectAsync(path);

        if (redirectResult.HasValue)
        {
            var (targetUrl, statusCode) = redirectResult.Value;

            // Preserve query string if the target URL doesn't have one
            if (!targetUrl.Contains('?') && context.Request.QueryString.HasValue)
            {
                targetUrl += context.Request.QueryString.Value;
            }

            _logger.LogInformation("Redirecting {SourceUrl} to {TargetUrl} with status code {StatusCode}",
                path, targetUrl, statusCode);

            context.Response.Redirect(targetUrl, statusCode == 301);
            return;
        }

        await _next(context);
    }
}

public static class RedirectMiddlewareExtensions
{
    public static IApplicationBuilder UseRedirectMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RedirectMiddleware>();
    }
}