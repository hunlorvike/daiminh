using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace web.Areas.Admin.Requests.ContentType;

/// <summary>
/// Represents a request to delete a content type.
/// </summary>
public class ContentTypeDeleteRequest
{
    /// <summary>
    /// Gets or sets the ID of the content type to delete.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Id không được bỏ trống")]
    public int Id { get; set; }
}

/// <summary>
/// Validator for <see cref="ContentTypeDeleteRequest"/>
/// </summary>
public class ContentTypeDeleteRequestValidator : AbstractValidator<ContentTypeDeleteRequest>
{
    private readonly ApplicationDbContext _dbContext;
    /// <summary>
    /// Initializer a new instance of the <see cref="ContentTypeDeleteRequestValidator"/> class.
    /// </summary>
    public ContentTypeDeleteRequestValidator(ApplicationDbContext context)
    {
        _dbContext = context;

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID loại nội dung phải là một số nguyên dương.")
            .MustAsync(BeExistingContentType).WithMessage("Loại nội dung không tồn tại hoặc đã bị xoá");
    }

    private async Task<bool> BeExistingContentType(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.ContentTypes.AnyAsync(x => x.Id == id && x.DeletedAt == null, cancellationToken);
    }
}