// Example using FluentValidation (not strictly necessary, but good practice)
using FluentValidation;

namespace web.Areas.Admin.Requests.Category;

/// <summary>
/// Represents a request to delete a category.
/// </summary>
public class CategoryDeleteRequest
{
    /// <summary>
    /// Gets or sets the ID of the category to delete.
    /// </summary>
    public int Id { get; set; }
}

/// <summary>
///  Validator for <see cref="CategoryDeleteRequest"/>
/// </summary>
public class CategoryDeleteRequestValidator : AbstractValidator<CategoryDeleteRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryDeleteRequestValidator"/> class.
    /// </summary>
    public CategoryDeleteRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID danh mục phải là một số nguyên dương.");
    }
}