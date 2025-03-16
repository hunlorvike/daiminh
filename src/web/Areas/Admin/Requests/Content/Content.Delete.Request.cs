using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace web.Areas.Admin.Requests.Content;

/// <summary>
/// Represents a request to delete content.
/// </summary>
public class ContentDeleteRequest
{
    /// <summary>
    /// Gets or sets the ID of the content to delete.
    /// </summary>
    /// <example>1</example>
    [Required]
    public int Id { get; set; }
}

/// <summary>
/// Validator for <see cref="ContentDeleteRequest"/>.
/// </summary>
public class ContentDeleteRequestValidator : AbstractValidator<ContentDeleteRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContentDeleteRequestValidator"/> class.
    /// </summary>
    public ContentDeleteRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID nội dung phải là một số nguyên dương.");
    }
}