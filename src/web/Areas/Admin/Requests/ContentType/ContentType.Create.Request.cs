using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace web.Areas.Admin.Requests.ContentType;

/// <summary>
/// Represents a request to create a new content type.
/// </summary>
public class ContentTypeCreateRequest
{
    /// <summary>
    /// Gets or sets the name of the content type.
    /// </summary>
    /// <example>Blog Post</example>
    [Display(Name = "Tên loại nội dung", Prompt = "Nhập tên loại nội dung")]
    [Required(ErrorMessage = "Tên loại nội dung không được bỏ trống.")] // Use DataAnnotations
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the slug (URL-friendly name) of the content type.
    /// </summary>
    /// <example>blog-post</example>
    [Display(Name = "Đường dẫn", Prompt = "Nhập đường dẫn")]
    [Required(ErrorMessage = "Đường dẫn loại nội dung không được bỏ trống.")] // Use DataAnnotations
    public string? Slug { get; set; }
}

/// <summary>
/// Validator for <see cref="ContentTypeCreateRequest"/>.
/// </summary>
public class ContentTypeCreateRequestValidator : AbstractValidator<ContentTypeCreateRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentTypeCreateRequestValidator"/> class.
    /// </summary>
    public ContentTypeCreateRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên loại nội dung không được bỏ trống.")
            .MaximumLength(50).WithMessage("Tên loại nội dung không được vượt quá 50 ký tự.");

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Đường dẫn (slug) không được bỏ trống.")
            .MaximumLength(50).WithMessage("Đường dẫn (slug) không được vượt quá 50 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Đường dẫn (slug) chỉ được chứa chữ cái thường, số và dấu gạch ngang (-), và không được bắt đầu hoặc kết thúc bằng dấu gạch ngang.")
            .MustAsync(BeUniqueSlug).WithMessage("Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác.");
    }

    /// <summary>
    /// Checks if the Slug is unique in the database.
    /// </summary>
    private async Task<bool> BeUniqueSlug(string slug, CancellationToken cancellationToken)
    {
        return !await _dbContext.ContentTypes
            .AnyAsync(ct => ct.Slug == slug && ct.DeletedAt == null, cancellationToken);
    }
}