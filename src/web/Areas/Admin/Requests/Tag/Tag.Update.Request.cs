using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Tag;

/// <summary>
/// Represents a request to update an existing tag.
/// </summary>
public class TagUpdateRequest
{
    /// <summary>
    /// Gets or sets the ID of the tag to update.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Id không được bỏ trống")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the updated name of the tag.
    /// </summary>
    /// <example>Updated Technology</example>
    [Display(Name = "Tên thẻ", Prompt = "Nhập tên thẻ")]
    [Required(ErrorMessage = "Tên thẻ là bắt buộc.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated slug (URL-friendly name) of the tag.
    /// </summary>
    /// <example>updated-technology</example>
    [Display(Name = "Đường dẫn", Prompt = "Nhập đường dẫn")]  // Changed to "Đường dẫn thẻ"
    [Required(ErrorMessage = "Đường dẫn thẻ là bắt buộc.")] // Changed to "Đường dẫn thẻ"
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the entity type of the tag.
    /// </summary>
    [Display(Name = "Loại thẻ", Prompt = "Chọn loại thẻ")]
    public EntityType EntityType { get; set; } = EntityType.Product;
}

/// <summary>
/// Validator for <see cref="TagUpdateRequest"/>.
/// </summary>
public class TagUpdateRequestValidator : AbstractValidator<TagUpdateRequest>
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="TagUpdateRequestValidator"/> class.
    /// </summary>
    public TagUpdateRequestValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(request => request.Id)
            .GreaterThan(0).WithMessage("ID tag phải là một số nguyên dương.")
            .MustAsync(BeExistingTag).WithMessage("Thẻ không tồn tại hoặc đã bị xóa.");

        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên thẻ không được bỏ trống.")
            .MaximumLength(50).WithMessage("Tên thẻ không được vượt quá 50 ký tự.");

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Đường dẫn (slug) không được bỏ trống.")
            .MaximumLength(50).WithMessage("Đường dẫn (slug) không được vượt quá 50 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Đường dẫn (slug) chỉ được chứa chữ cái thường, số và dấu gạch ngang (-), và không được bắt đầu hoặc kết thúc bằng dấu gạch ngang.")
            .MustAsync(BeUniqueSlug).WithMessage("Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác.");

        RuleFor(request => request.EntityType)
            .IsInEnum().WithMessage("Loại thẻ không hợp lệ.");
    }

    /// <summary>
    /// Checks if the tag exists and is not deleted.
    /// </summary>
    private async Task<bool> BeExistingTag(int id, CancellationToken cancellationToken)
    {
        return await _context.Tags
            .AnyAsync(t => t.Id == id && t.DeletedAt == null, cancellationToken);
    }

    /// <summary>
    /// Checks if the slug is unique (excluding the current tag).
    /// </summary>
    private async Task<bool> BeUniqueSlug(TagUpdateRequest request, string slug, CancellationToken cancellationToken)
    {
        return !await _context.Tags
            .AnyAsync(t => t.Slug == slug && t.Id != request.Id && t.DeletedAt == null, cancellationToken);
    }
}