using System.Net;
using System.Text.Json;
using shared.Exceptions;
using shared.Models;

namespace web.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred");

        bool isApiRequest = context.Request.Path.StartsWithSegments("/api") ||
                            context.Request.Headers["Accept"].Any(h => h.Contains("application/json"));

        if (isApiRequest)
        {
            await HandleApiExceptionAsync(context, exception);
        }
        else
        {
            await HandleWebExceptionAsync(context, exception);
        }
    }

    private async Task HandleApiExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new ErrorResponse
        {
            TraceId = context.TraceIdentifier
        };

        switch (exception)
        {
            case AppException appException:
                response.StatusCode = (int)appException.StatusCode;
                errorResponse.Message = appException.Message;
                errorResponse.ErrorCode = appException.ErrorCode;

                if (appException is ValidationException validationException)
                {
                    errorResponse.Errors = validationException.Errors;
                }

                if (appException.AdditionalData != null)
                {
                    errorResponse.AdditionalData = appException.AdditionalData;
                }
                break;

            case KeyNotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Message = "The requested resource was not found.";
                errorResponse.ErrorCode = "RESOURCE_NOT_FOUND";
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = _env.IsDevelopment()
                    ? exception.Message
                    : "An unexpected error occurred.";
                errorResponse.ErrorCode = "INTERNAL_ERROR";
                break;
        }

        if (_env.IsDevelopment())
        {
            errorResponse.DeveloperMessage = exception.ToString();
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(errorResponse, jsonOptions);
        await response.WriteAsync(json);
    }

    private async Task HandleWebExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;

        // Set status code
        response.StatusCode = exception switch
        {
            NotFoundException => (int)HttpStatusCode.NotFound,
            UnauthorizedException => (int)HttpStatusCode.Unauthorized,
            ForbiddenException => (int)HttpStatusCode.Forbidden,
            BadRequestException or ValidationException => (int)HttpStatusCode.BadRequest,
            AppException appException => (int)appException.StatusCode,
            _ => (int)HttpStatusCode.InternalServerError
        };

        // Store exception details in TempData for the error view
        var errorViewModel = new ErrorViewModel
        {
            RequestId = context.TraceIdentifier,
            StatusCode = response.StatusCode,
            Message = _env.IsDevelopment() ? exception.Message : "An error occurred while processing your request."
        };

        if (_env.IsDevelopment())
        {
            errorViewModel.Exception = exception;
        }

        // Redirect to the appropriate error page
        context.Items["ErrorViewModel"] = errorViewModel;

        switch (response.StatusCode)
        {
            case (int)HttpStatusCode.NotFound:
                response.Redirect("/Error/NotFound");
                break;
            case (int)HttpStatusCode.Unauthorized:
                response.Redirect("/Error/Unauthorized");
                break;
            case (int)HttpStatusCode.Forbidden:
                response.Redirect("/Error/Forbidden");
                break;
            default:
                response.Redirect("/Error");
                break;
        }
    }
}

// Extension method to add the middleware to the HTTP request pipeline
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}

