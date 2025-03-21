using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

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
    private readonly ApplicationDbContext _dbContext;
    /// <summary>
    /// Initializes a new instance of the <see cref="ContentDeleteRequestValidator"/> class.
    /// </summary>
    public ContentDeleteRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID nội dung phải là một số nguyên dương.")
            .MustAsync(BeExistingContent).WithMessage("Nội dung không tồn tại hoặc đã bị xoá");
    }

    private async Task<bool> BeExistingContent(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Contents
            .AnyAsync(s => s.Id == id && s.DeletedAt == null, cancellationToken);
    }
}