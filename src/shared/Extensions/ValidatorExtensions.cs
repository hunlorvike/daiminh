using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace shared.Extensions;

public static class ValidatorExtensions
{
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

        return controller.BadRequest(errors);
    }
}