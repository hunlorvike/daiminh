using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Enums;

namespace web.Areas.Admin.Requests.Category;

/// <summary>
/// Represents a request to create a new category.
/// </summary>
public class CategoryCreateRequest
{
    /// <summary>
    /// Gets or sets the name of the category.
    /// </summary>
    /// <example>Electronics</example>
    [Display(Name = "Tên danh mục", Prompt = "Nhập tên danh mục")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the slug (URL-friendly name) of the category.
    /// </summary>
    /// <example>electronics</example>
    [Display(Name = "Đường dẫn", Prompt = "Nhập đường dẫn")]
    public string? Slug { get; set; }

    /// <summary>
    /// Gets or sets the ID of the parent category.  Set to null if this is a top-level category.
    /// </summary>
    /// <example>1</example>
    [Display(Name = "Danh mục cha", Prompt = "Chọn danh mục cha")]
    public int? ParentCategoryId { get; set; }

    /// <summary>
    /// Gets or sets the entity type of the tag.
    /// </summary>
    [Display(Name = "Loại danh mục", Prompt = "Chọn loại danh mục")]
    public EntityType EntityType { get; set; } = EntityType.Product;

}

/// <summary>
/// Validator for <see cref="CategoryCreateRequest"/>.
/// </summary>
public class CategoryCreateRequestValidator : AbstractValidator<CategoryCreateRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryCreateRequestValidator"/> class.
    /// </summary>
    public CategoryCreateRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên danh mục không được bỏ trống.")
            .MaximumLength(100).WithMessage("Tên danh mục không được vượt quá 100 ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Đường dẫn (slug) không được bỏ trống.")
            .MaximumLength(100).WithMessage("Đường dẫn (slug) không được vượt quá 100 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Đường dẫn (slug) chỉ được chứa chữ cái thường, số và dấu gạch ngang (-), và không được bắt đầu hoặc kết thúc bằng dấu gạch ngang.")
            .MustAsync(BeUniqueSlug).WithMessage("Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác.");

        RuleFor(x => x.ParentCategoryId)
            .GreaterThan(0).When(x => x.ParentCategoryId.HasValue)
            .WithMessage("ID danh mục cha, nếu được cung cấp, phải là một số nguyên dương.")
            .MustAsync(BeValidParentCategory).When(x => x.ParentCategoryId.HasValue)
            .WithMessage("Danh mục cha không tồn tại hoặc đã bị xóa.");

        RuleFor(x => x.EntityType)
            .IsInEnum().WithMessage("Loại danh mục không hợp lệ.");
    }

    /// <summary>
    /// Checks if the slug is unique in the database.
    /// </summary>
    private async Task<bool> BeUniqueSlug(string slug, CancellationToken cancellationToken)
    {
        return !await _dbContext.Categories
            .AnyAsync(c => c.Slug == slug && c.DeletedAt == null, cancellationToken);
    }

    /// <summary>
    /// Checks if the ParentCategoryId is valid and exists in the database.
    /// </summary>
    private async Task<bool> BeValidParentCategory(int? parentCategoryId, CancellationToken cancellationToken)
    {
        if (!parentCategoryId.HasValue) return true;

        return await _dbContext.Categories
            .AnyAsync(c => c.Id == parentCategoryId && c.DeletedAt == null, cancellationToken);
    }
}