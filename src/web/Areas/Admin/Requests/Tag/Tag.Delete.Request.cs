using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace web.Areas.Admin.Requests.Tag;

/// <summary>
/// Represents a request to delete a tag.
/// </summary>
public class TagDeleteRequest
{
    /// <summary>
    /// Gets or sets the ID of the tag to delete.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Id không được bỏ trống")]
    public int Id { get; set; }
}

/// <summary>
/// Validator for <see cref="TagDeleteRequest"/>
/// </summary>
public class TagDeleteRequestValidator : AbstractValidator<TagDeleteRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializer a new instance of the <see cref="TagDeleteRequestValidator"/> class.
    /// </summary>
    public TagDeleteRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID tag phải là một số nguyên dương.")
            .MustAsync(BeExistingTag).WithMessage("Thẻ không tồn tại hoặc đã bị xoá");
    }

    private async Task<bool> BeExistingTag(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Tags
            .AnyAsync(t => t.Id == id && t.DeletedAt == null, cancellationToken);
    }
}