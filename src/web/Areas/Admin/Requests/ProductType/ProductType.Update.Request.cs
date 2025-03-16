using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace web.Areas.Admin.Requests.ProductType;

/// <summary>
/// Represents a request to update an existing product type.
/// </summary>
public class ProductTypeUpdateRequest
{
    /// <summary>
    /// Gets or sets the ID of the product type to update.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Id không được bỏ trống")]
    public int Id { get; set; }

    /// <summary
    /// Gets or sets the updated name of the product type.
    /// </summary>
    /// <example>Gaming Laptop</example>
    [Display(Name = "Tên loại sản phẩm", Prompt = "Nhập tên loại sản phẩm")]
    [Required(ErrorMessage = "Tên loại sản phẩm không được bỏ trống.")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the updated slug (URL-friendly name) of the product type.
    /// </summary>
    /// <example>gaming-laptop</example>
    [Display(Name = "Đường dẫn", Prompt = "Nhập đường dẫn")]
    [Required(ErrorMessage = "Đường dẫn loại sản phẩm không được bỏ trống.")]
    public string? Slug { get; set; }
}

/// <summary>
/// Validator for <see cref="ProductTypeUpdateRequest"/>.
/// </summary>
public class ProductTypeUpdateRequestValidator : AbstractValidator<ProductTypeUpdateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductTypeUpdateRequestValidator"/> class.
    /// </summary>
    public ProductTypeUpdateRequestValidator()
    {
        RuleFor(request => request.Id)
            .GreaterThan(0).WithMessage("ID loại sản phẩm phải là một số nguyên dương.");

        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên loại sản phẩm không được bỏ trống.") // Improved message
            .MaximumLength(50).WithMessage("Tên loại sản phẩm không được vượt quá 50 ký tự."); // Match entity

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Đường dẫn (slug) không được bỏ trống.") // Improved message and terminology
            .MaximumLength(50).WithMessage("Đường dẫn (slug) không được vượt quá 50 ký tự.") // Match entity
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Đường dẫn (slug) chỉ được chứa chữ cái thường, số và dấu gạch ngang (-), và không được bắt đầu hoặc kết thúc bằng dấu gạch ngang."); // More descriptive
    }
}