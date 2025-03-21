using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

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
    private readonly ApplicationDbContext _dbContext;
    /// <summary>
    ///  Initializes a new instance of the <see cref="ContentFieldDefinitionDeleteRequestValidator"/> class.
    /// </summary>
    public ContentFieldDefinitionDeleteRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID trường phải là một số nguyên dương.")
            .MustAsync(BeExistingContentFieldDefinition).WithMessage("Trường không tồn tại hoặc đã bị xoá");
    }

    private async Task<bool> BeExistingContentFieldDefinition(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.ContentFieldDefinitions
            .AnyAsync(s => s.Id == id && s.DeletedAt == null, cancellationToken);
    }
}