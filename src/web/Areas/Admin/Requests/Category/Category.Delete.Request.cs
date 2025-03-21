using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace web.Areas.Admin.Requests.Category;

/// <summary>
/// Represents a request to delete a category.
/// </summary>
public class CategoryDeleteRequest
{
    /// <summary>
    /// Gets or sets the ID of the category to delete.
    /// </summary>
    public int Id { get; set; }
}


/// <summary>
/// Validator for <see cref="CategoryDeleteRequest"/>.
/// </summary>
public class CategoryDeleteRequestValidator : AbstractValidator<CategoryDeleteRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryDeleteRequestValidator"/> class.
    /// </summary>
    public CategoryDeleteRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID danh mục phải là một số nguyên dương.")
            .MustAsync(BeExistingCategory).WithMessage("Danh mục không tồn tại hoặc đã bị xóa.");
    }

    private async Task<bool> BeExistingCategory(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Categories
            .AnyAsync(c => c.Id == id && c.DeletedAt == null, cancellationToken);
    }
}