using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Category;

/// <summary>
/// Represents a request to update an existing category.
/// </summary>
public class CategoryUpdateRequest
{
    /// <summary>
    /// Gets or sets the ID of the category to update.
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the updated name of the category.
    /// </summary>
    /// <example>Updated Electronics</example>
    [Display(Name = "Tên danh mục", Prompt = "Nhập tên danh mục")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the updated slug (URL-friendly name) of the category.
    /// </summary>
    /// <example>updated-electronics</example>
    [Display(Name = "Đường dẫn", Prompt = "Nhập đường dẫn")]
    public string? Slug { get; set; }

    /// <summary>
    /// Gets or sets the ID of the parent category. Set to null to remove the parent category.
    /// </summary>
    /// <example>2</example>
    [Display(Name = "Danh mục cha", Prompt = "Chọn danh mục cha")]
    public int? ParentCategoryId { get; set; }

    /// <summary>
    /// Gets or sets the entity type of the tag.
    /// </summary>
    [Display(Name = "Loại danh mục", Prompt = "Chọn loại danh mục")]
    public EntityType EntityType { get; set; } = EntityType.Product;
}

/// <summary>
/// Validator for <see cref="CategoryUpdateRequest"/>.
/// </summary>
public class CategoryUpdateRequestValidator : AbstractValidator<CategoryUpdateRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryUpdateRequestValidator"/> class.
    /// </summary>
    public CategoryUpdateRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(request => request.Id)
            .GreaterThan(0).WithMessage("ID danh mục phải là một số nguyên dương.")
            .MustAsync(BeExistingCategory).WithMessage("Danh mục không tồn tại hoặc đã bị xóa.");

        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên danh mục không được bỏ trống.")
            .MaximumLength(100).WithMessage("Tên danh mục không được vượt quá 100 ký tự.");

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Đường dẫn (slug) không được bỏ trống.")
            .MaximumLength(100).WithMessage("Đường dẫn (slug) không được vượt quá 100 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Đường dẫn (slug) chỉ được chứa chữ cái thường, số và dấu gạch ngang (-), và không được bắt đầu hoặc kết thúc bằng dấu gạch ngang.")
            .MustAsync(BeUniqueSlug).WithMessage("Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác.");

        RuleFor(x => x.ParentCategoryId)
            .GreaterThan(0).When(x => x.ParentCategoryId.HasValue)
            .WithMessage("ID danh mục cha, nếu được cung cấp, phải là một số nguyên dương.")
            .MustAsync(BeValidParentCategory).When(x => x.ParentCategoryId.HasValue)
            .WithMessage("Danh mục cha không tồn tại, đã bị xóa hoặc không hợp lệ.");

        RuleFor(x => x.EntityType)
            .IsInEnum().WithMessage("Loại danh mục không hợp lệ.");
    }

    /// <summary>
    /// Checks if the category exists and is not deleted.
    /// </summary>
    private async Task<bool> BeExistingCategory(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Categories
            .AnyAsync(c => c.Id == id && c.DeletedAt == null, cancellationToken);
    }

    /// <summary>
    /// Checks if the slug is unique (excluding the current category).
    /// </summary>
    private async Task<bool> BeUniqueSlug(CategoryUpdateRequest request, string slug, CancellationToken cancellationToken)
    {
        return !await _dbContext.Categories
            .AnyAsync(c => c.Slug == slug && c.Id != request.Id && c.DeletedAt == null, cancellationToken);
    }

    /// <summary>
    /// Checks if the ParentCategoryId is valid, exists, and is not the category itself or its descendant.
    /// </summary>
    private async Task<bool> BeValidParentCategory(CategoryUpdateRequest request, int? parentCategoryId, CancellationToken cancellationToken)
    {
        if (!parentCategoryId.HasValue) return true;

        var parentExists = await _dbContext.Categories
            .AnyAsync(c => c.Id == parentCategoryId && c.DeletedAt == null, cancellationToken);

        if (!parentExists) return false;

        if (parentCategoryId == request.Id) return false;

        var currentId = parentCategoryId.Value;
        while (currentId != 0)
        {
            var category = await _dbContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == currentId && c.DeletedAt == null, cancellationToken);

            if (category == null) return true; 
            if (category.Id == request.Id) return false;
            currentId = category.ParentCategoryId ?? 0;
        }

        return true;
    }
}