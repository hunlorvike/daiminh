using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.ContentFieldDefinition;

/// <summary>
/// Represents a request to delete a content field definition.
/// </summary>
public class ContentFieldDefinitionDeleteRequest
{
    /// <summary>
    /// Gets or sets the ID of the field definition to delete.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Id không được bỏ trống")]
    public int Id { get; set; }
}

/// <summary>
/// Validator for <see cref="ContentFieldDefinitionDeleteRequest"/>.
/// </summary>
public class ContentFieldDefinitionDeleteRequestValidator : AbstractValidator<ContentFieldDefinitionDeleteRequest>
{
    /// <summary>
    ///  Initializes a new instance of the <see cref="ContentFieldDefinitionDeleteRequestValidator"/> class.
    /// </summary>
    public ContentFieldDefinitionDeleteRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID trường phải là một số nguyên dương.");
    }
}