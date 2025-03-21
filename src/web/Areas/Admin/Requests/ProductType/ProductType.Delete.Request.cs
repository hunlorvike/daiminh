using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace web.Areas.Admin.Requests.ProductType;

/// <summary>
/// Represents a request to delete a product type.
/// </summary>
public class ProductTypeDeleteRequest
{
    /// <summary>
    /// Gets or sets the ID of the product type to delete.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Id không được bỏ trống")]
    public int Id { get; set; }
}

/// <summary>
/// Validator for <see cref="ProductTypeDeleteRequest"/>
/// </summary>
public class ProductTypeDeleteRequestValidator : AbstractValidator<ProductTypeDeleteRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializer a new instance of the <see cref="ProductTypeDeleteRequestValidator"/> class.
    /// </summary>
    public ProductTypeDeleteRequestValidator(ApplicationDbContext context)
    {
        _dbContext = context;

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID loại sản phẩm phải là một số nguyên dương.")
            .MustAsync(BeExistingProductType).WithMessage("Loại sản phẩm không tồn tại hoặc đã bị xoá");
    }

    private async Task<bool> BeExistingProductType(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.ProductTypes
            .AnyAsync(s => s.Id == id && s.DeletedAt == null, cancellationToken);
    }
}