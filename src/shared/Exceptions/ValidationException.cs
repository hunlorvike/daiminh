using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace shared.Exceptions;

public class ValidationException : AppException
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(string message)
        : base(message, HttpStatusCode.BadRequest, "VALIDATION_FAILED")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation errors occurred", HttpStatusCode.BadRequest, "VALIDATION_FAILED")
    {
        Errors = errors;
    }

    public ValidationException(ModelStateDictionary modelState)
        : base("One or more validation errors occurred", HttpStatusCode.BadRequest, "VALIDATION_FAILED")
    {
        Errors = modelState
            .Where(x => x.Value?.Errors?.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );
    }
}
