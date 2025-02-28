using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace web.Areas.Admin.Requests.Category;

public class CategoryCreateRequest
{
    [Display(Name = "Tên danh mục", Prompt = "Nhập tên của danh mục")]
    public string? Name { get; set; }

    [Display(Name = "Đường dẫn danh mục", Prompt = "Nhập đường dẫn của danh mục")]
    public string? Slug { get; set; }

    [Display(Name = "ID danh mục cha", Prompt = "Nhập ID danh mục cha (Không chọn nếu không có)")]
    public int? ParentCategoryId { get; set; }
}

public class CategoryUpdateRequest
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên không được để trống.")]
    [Display(Name = "Tên danh mục", Prompt = "Nhập tên của danh mục")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Slug không được để trống.")]
    [Display(Name = "Đường dẫn danh mục", Prompt = "Nhập đường dẫn của danh mục")]
    public string? Slug { get; set; }

    [Display(Name = "ID danh mục cha", Prompt = "Nhập ID danh mục cha (Không chọn nếu không có)")]
    public int? ParentCategoryId { get; set; }
}

public class CategoryDeleteRequest
{
    public int Id { get; set; }
}

public class CategoryCreateRequestValidator : AbstractValidator<CategoryCreateRequest>
{
    public CategoryCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên không được để trống.")
            .MaximumLength(255).WithMessage("Tên không được quá 255 ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug không được để trống.")
            .MaximumLength(255).WithMessage("Slug không được quá 255 ký tự.");
    }
}

public class CategoryUpdateRequestValidator : AbstractValidator<CategoryUpdateRequest>
{
    public CategoryUpdateRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên không được để trống.")
            .MaximumLength(255).WithMessage("Tên không được vượt quá 255 ký tự.");

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Slug không được để trống.")
            .MaximumLength(255).WithMessage("Slug không được vượt quá 255 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang.")
            // .MustAsync(BeUniqueSlug).WithMessage("Slug đã tồn tại.");
            ;
    }
}