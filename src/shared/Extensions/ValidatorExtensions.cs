using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using shared.Models;

namespace shared.Extensions;

/// <summary>
/// Provides extension methods for integrating FluentValidation with ASP.NET Core MVC controllers.
/// </summary>
public static class ValidatorExtensions
{
    /// <summary>
    /// Validates a model using a specified FluentValidation validator and returns a BadRequestObjectResult
    /// with an <see cref="ErrorResponse"/> if validation fails.  If validation succeeds, returns null.
    /// </summary>
    /// <typeparam name="T">The type of the model to validate.</typeparam>
    /// <param name="controller">The controller instance.</param>
    /// <param name="validator">The FluentValidation validator.</param>
    /// <param name="model">The model to validate.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> representing the validation result. Returns null if validation is successful.
    /// Returns a BadRequestObjectResult with an <see cref="ErrorResponse"/> containing validation errors if validation fails.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="validator"/> or <paramref name="model"/> is null.</exception>
    public static async Task<IActionResult?> ValidateAndReturnBadRequest<T>(
        this Controller controller,
        IValidator<T> validator,
        T model) where T : class
    {
        ArgumentNullException.ThrowIfNull(validator);
        ArgumentNullException.ThrowIfNull(model);

        var validationResult = await validator.ValidateAsync(model);

        if (validationResult.IsValid) return null;

        Dictionary<string, string[]> errors = validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(e => e.ErrorMessage).ToArray()
            );

        return controller.BadRequest(new ErrorResponse(errors));
    }
}