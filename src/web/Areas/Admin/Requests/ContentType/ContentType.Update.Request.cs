using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace web.Areas.Admin.Requests.ContentType;

/// <summary>
/// Represents a request to update an existing content type.
/// </summary>
public class ContentTypeUpdateRequest
{
    /// <summary>
    /// Gets or sets the ID of the content type to update.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Id không được bỏ trống")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the updated name of the content type.
    /// </summary>
    /// <example>News Article</example>
    [Display(Name = "Tên loại nội dung", Prompt = "Nhập tên loại nội dung")]
    [Required(ErrorMessage = "Tên loại nội dung không được bỏ trống.")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the updated slug (URL-friendly name) of the content type.
    /// </summary>
    /// <example>news-article</example>
    [Display(Name = "Đường dẫn", Prompt = "Nhập đường dẫn")]
    [Required(ErrorMessage = "Đường dẫn loại nội dung không được bỏ trống.")]
    public string? Slug { get; set; }
}

/// <summary>
/// Validator for <see cref="ContentTypeUpdateRequest"/>.
/// </summary>
public class ContentTypeUpdateRequestValidator : AbstractValidator<ContentTypeUpdateRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentTypeUpdateRequestValidator"/> class.
    /// </summary>
    public ContentTypeUpdateRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(request => request.Id)
            .GreaterThan(0).WithMessage("ID loại nội dung phải là một số nguyên dương.")
            .MustAsync(BeExistingContentType).WithMessage("Loại nội dung không tồn tại hoặc đã bị xóa.");

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
    /// Checks if the ContentType exists and is not deleted.
    /// </summary>
    private async Task<bool> BeExistingContentType(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.ContentTypes
            .AnyAsync(ct => ct.Id == id && ct.DeletedAt == null, cancellationToken);
    }

    /// <summary>
    /// Checks if the Slug is unique (excluding the current content type).
    /// </summary>
    private async Task<bool> BeUniqueSlug(ContentTypeUpdateRequest request, string slug, CancellationToken cancellationToken)
    {
        return !await _dbContext.ContentTypes
            .AnyAsync(ct => ct.Slug == slug && ct.Id != request.Id && ct.DeletedAt == null, cancellationToken);
    }
}