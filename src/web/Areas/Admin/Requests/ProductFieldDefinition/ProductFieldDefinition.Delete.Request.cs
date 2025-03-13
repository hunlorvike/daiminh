using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.ProductFieldDefinition;

/// <summary>
/// Represents a request to delete a product field definition.
/// </summary>
public class ProductFieldDefinitionDeleteRequest
{
    /// <summary>
    /// Gets or sets the ID of the field definition to delete.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Id không được bỏ trống")]
    public int Id { get; set; }
}

/// <summary>
/// Validator for <see cref="ProductFieldDefinitionDeleteRequest"/>
/// </summary>
public class ProductFieldDefinitionDeleteRequestValidator : AbstractValidator<ProductFieldDefinitionDeleteRequest>
{
    /// <summary>
    /// Initializer a new instance of the <see cref="ProductFieldDefinitionDeleteRequestValidator"/> class.
    /// </summary>
    public ProductFieldDefinitionDeleteRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID trường phải là một số nguyên dương.");
    }
}