using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

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
    [Display(Name = "Tên loại sản phẩm", Prompt = "Nhập tên loại sản phẩm")]
    [Required(ErrorMessage = "Tên loại sản phẩm không được bỏ trống.")] //DataAnnotations
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the slug (URL-friendly name) of the product type.
    /// </summary>
    /// <example>laptop</example>
    [Display(Name = "Đường dẫn", Prompt = "Nhập đường dẫn")]
    [Required(ErrorMessage = "Đường dẫn loại sản phẩm không được bỏ trống.")] //DataAnnotations
    public string? Slug { get; set; }
}

/// <summary>
/// Validator for <see cref="ProductTypeCreateRequest"/>.
/// </summary>
public class ProductTypeCreateRequestValidator : AbstractValidator<ProductTypeCreateRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductTypeCreateRequestValidator"/> class.
    /// </summary>
    public ProductTypeCreateRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên loại sản phẩm không được bỏ trống.")
            .MaximumLength(50).WithMessage("Tên loại sản phẩm không được vượt quá 50 ký tự.")
            .MustAsync(BeUniqueName).WithMessage("Tên loại sản phẩm đã tồn tại. Vui lòng chọn một tên khác.");

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Đường dẫn (slug) không được bỏ trống.")
            .MaximumLength(50).WithMessage("Đường dẫn (slug) không được vượt quá 50 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Đường dẫn (slug) chỉ được chứa chữ cái thường, số và dấu gạch ngang (-), và không được bắt đầu hoặc kết thúc bằng dấu gạch ngang.")
            .MustAsync(BeUniqueSlug).WithMessage("Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác.");
    }

    /// <summary>
    /// Checks if the Name is unique in the database.
    /// </summary>
    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return !await _dbContext.ProductTypes
            .AnyAsync(pt => pt.Name == name && pt.DeletedAt == null, cancellationToken);
    }

    /// <summary>
    /// Checks if the Slug is unique in the database.
    /// </summary>
    private async Task<bool> BeUniqueSlug(string slug, CancellationToken cancellationToken)
    {
        return !await _dbContext.ProductTypes
            .AnyAsync(pt => pt.Slug == slug && pt.DeletedAt == null, cancellationToken);
    }
}