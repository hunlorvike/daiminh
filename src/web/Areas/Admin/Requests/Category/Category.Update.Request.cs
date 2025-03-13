using FluentValidation;
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
    [Display(Name = "Tên danh mục", Prompt = "Nhập tên của danh mục")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the updated slug (URL-friendly name) of the category.
    /// </summary>
    /// <example>updated-electronics</example>
    [Display(Name = "Đường dẫn danh mục", Prompt = "Nhập đường dẫn của danh mục")]
    public string? Slug { get; set; }

    /// <summary>
    /// Gets or sets the ID of the parent category. Set to null to remove the parent category.
    /// </summary>
    /// <example>2</example>
    [Display(Name = "ID danh mục cha", Prompt = "Nhập ID danh mục cha (Không chọn nếu không có)")]
    public int? ParentCategoryId { get; set; }
}

/// <summary>
/// Validator for <see cref="CategoryUpdateRequest"/>.
/// </summary>
public class CategoryUpdateRequestValidator : AbstractValidator<CategoryUpdateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryUpdateRequestValidator"/> class.
    /// </summary>
    public CategoryUpdateRequestValidator()
    {
        RuleFor(request => request.Id)  // Validate the ID
            .GreaterThan(0).WithMessage("ID danh mục phải là một số nguyên dương.");

        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên danh mục không được bỏ trống.") // Improved message
            .MaximumLength(100).WithMessage("Tên danh mục không được vượt quá 100 ký tự.");

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Đường dẫn (slug) không được bỏ trống.") // Improved message and terminology
            .MaximumLength(100).WithMessage("Đường dẫn (slug) không được vượt quá 100 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$") // Regex for valid slugs
            .WithMessage("Đường dẫn (slug) chỉ được chứa chữ cái thường, số và dấu gạch ngang (-), và không được bắt đầu hoặc kết thúc bằng dấu gạch ngang.");

        RuleFor(x => x.ParentCategoryId)
            .GreaterThan(0).When(x => x.ParentCategoryId.HasValue)  // Only check if a value is provided
            .WithMessage("ID danh mục cha, nếu được cung cấp, phải là một số nguyên dương.");
    }
}