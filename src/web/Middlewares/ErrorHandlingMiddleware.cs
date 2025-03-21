using System.Net;
using System.Text.Json;
using Serilog;
using shared.Models;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
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
        (HttpStatusCode statusCode, string message, LogLevel logLevel) = exception switch
        {
            NotFoundException ex => (HttpStatusCode.NotFound, ex.Message, LogLevel.Information),
            ValidationException ex => (HttpStatusCode.BadRequest, ex.Message, LogLevel.Warning),
            BusinessLogicException ex => (HttpStatusCode.UnprocessableEntity, ex.Message, LogLevel.Warning),
            SystemException2 ex => (HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.", LogLevel.Error),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.", LogLevel.Error)
        };

        if (logLevel == LogLevel.Error)
            Log.Error(exception, "Error occurred: {Message}", exception.Message);
        else if (logLevel == LogLevel.Warning)
            Log.Warning(exception, "Warning: {Message}", exception.Message);
        else
            Log.Information("Info: {Message}", exception.Message);

        var response = new
        {
            statusCode = (int)statusCode,
            message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}