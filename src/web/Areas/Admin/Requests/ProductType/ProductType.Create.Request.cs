using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace web.Areas.Admin.Requests.ProductType;

/// <summary>
/// Represents a request to create a new product type.
/// </summary>
public class ProductTypeCreateRequest
{
    /// <summary>
    /// Gets or sets the name of the product type.
    /// </summary>
    /// <example>Laptop</example>
    [Display(Name = "Tên loại sản phẩm", Prompt = "Nhập tên của loại sản phẩm")]
    [Required(ErrorMessage = "Tên loại sản phẩm không được bỏ trống.")] //DataAnnotations
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the slug (URL-friendly name) of the product type.
    /// </summary>
    /// <example>laptop</example>
    [Display(Name = "Đường dẫn loại sản phẩm", Prompt = "Nhập đường dẫn của loại sản phẩm")]
    [Required(ErrorMessage = "Đường dẫn loại sản phẩm không được bỏ trống.")] //DataAnnotations
    public string? Slug { get; set; }
}

/// <summary>
/// Validator for <see cref="ProductTypeCreateRequest"/>.
/// </summary>
public class ProductTypeCreateRequestValidator : AbstractValidator<ProductTypeCreateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductTypeCreateRequestValidator"/> class.
    /// </summary>
    public ProductTypeCreateRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên loại sản phẩm không được bỏ trống.") // Improved message
            .MaximumLength(50).WithMessage("Tên loại sản phẩm không được vượt quá 50 ký tự."); // Match entity

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Đường dẫn (slug) không được bỏ trống.") // Improved message and terminology
            .MaximumLength(50).WithMessage("Đường dẫn (slug) không được vượt quá 50 ký tự.") // Match entity
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Đường dẫn (slug) chỉ được chứa chữ cái thường, số và dấu gạch ngang (-), và không được bắt đầu hoặc kết thúc bằng dấu gạch ngang."); // More descriptive
    }
}