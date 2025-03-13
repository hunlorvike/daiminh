using application.Interfaces;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

/// <summary>
/// Service for managing categories.
/// </summary>
public partial class CategoryService(ApplicationDbContext context) : ICategoryService
{
    /// <summary>
    /// Retrieves all categories.
    /// </summary>
    /// <returns>A list of categories.</returns>
    public async Task<List<Category>> GetAllAsync()
    {
        try
        {
            // Retrieve all categories that are not soft-deleted, including their parent categories.
            return await context.Categories
                .AsNoTracking() // Improve performance for read-only scenarios
                .Where(c => c.DeletedAt == null)
                .Include(c => c.ParentCategory)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            // Log the exception.
            //_logger.LogError(ex, "Error occurred while retrieving all categories.");
            throw new Exception("Đã xảy ra lỗi khi lấy danh sách danh mục.", ex); // More general message
        }
    }

    /// <summary>
    /// Retrieves a category by its ID.
    /// </summary>
    /// <param name="id">The ID of the category to retrieve.</param>
    /// <returns>The category if found, or null otherwise.</returns>
    public async Task<Category?> GetByIdAsync(int id)
    {
        try
        {
            // Retrieve a category by ID, including its parent category.
            return await context.Categories
                .Where(c => c.Id == id && c.DeletedAt == null)
                .Include(c => c.ParentCategory)
                .AsNoTracking() // Use AsNoTracking for read-only operations
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            // Log the exception.
            // _logger.LogError(ex, $"Error occurred while retrieving category with ID: {id}.");
            throw new Exception($"Đã xảy ra lỗi khi lấy thông tin danh mục có ID: {id}.", ex);  // Include ID in message
        }
    }

    /// <summary>
    /// Adds a new category.
    /// </summary>
    /// <param name="model">The category model to add.</param>
    /// <returns>A BaseResponse indicating the result of the operation.</returns>
    public async Task<BaseResponse> AddAsync(Category model)
    {
        try
        {
            var errors = new Dictionary<string, string[]>();

            // Check if a category with the same slug already exists (and is not soft-deleted).
            var existingCategory = await context.Categories
                .FirstOrDefaultAsync(c => c.Slug == model.Slug && c.DeletedAt == null);

            if (existingCategory != null)
                errors.Add(nameof(model.Slug), ["Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác."]);

            // Return an error response if any validation errors were found.
            if (errors.Count != 0)
                return new ErrorResponse(errors);

            // Add the new category to the database.
            await context.Categories.AddAsync(model);
            await context.SaveChangesAsync();

            return new SuccessResponse<Category>(model, "Thêm danh mục mới thành công.");
        }
        catch (Exception ex)
        {
            // Log the exception.
            // _logger.LogError(ex, "Error occurred while adding a new category.");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi thêm danh mục mới. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="id">The ID of the category to update.</param>
    /// <param name="model">The updated category model.</param>
    /// <returns>A BaseResponse indicating the result of the operation.</returns>
    public async Task<BaseResponse> UpdateAsync(int id, Category model)
    {
        try
        {
            // Check if a category with the same slug already exists (excluding the current category).
            var existingSlug = await context.Categories
                .FirstOrDefaultAsync(ct => ct.Slug == model.Slug && ct.Id != id && ct.DeletedAt == null); // Exclude deleted

            if (existingSlug != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Slug), ["Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác."] }
                });

            // Find the existing category by ID.
            var existingCategory = await context.Categories
                .FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null);  // Check if it's not deleted

            if (existingCategory == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Danh mục không tồn tại hoặc đã bị xóa."] }
                });

            // Update the category properties.
            existingCategory.Name = model.Name ?? existingCategory.Name; // Use null-coalescing operator
            existingCategory.Slug = model.Slug ?? existingCategory.Slug; // Use null-coalescing operator
            existingCategory.ParentCategoryId = model.ParentCategoryId; // Allow nulls

            // Save the changes to the database.
            await context.SaveChangesAsync();

            return new SuccessResponse<Category>(existingCategory, "Cập nhật danh mục thành công.");
        }
        catch (Exception ex)
        {
            // Log the exception.
            // _logger.LogError(ex, $"Error occurred while updating category with ID: {id}.");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi cập nhật danh mục. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Deletes a category (soft delete).
    /// </summary>
    /// <param name="id">The ID of the category to delete.</param>
    /// <returns>A BaseResponse indicating the result of the operation.</returns>
    public async Task<BaseResponse> DeleteAsync(int id)
    {
        try
        {
            // Find the category by ID (only if not already soft-deleted).
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

            if (category == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Danh mục không tồn tại hoặc đã bị xóa."] } });

            // Perform a soft delete by setting the DeletedAt property.
            category.DeletedAt = DateTime.UtcNow;

            // Save the changes to the database.
            await context.SaveChangesAsync();

            return new SuccessResponse<Category>(category, "Xóa danh mục thành công (đã ẩn).");
        }
        catch (Exception ex)
        {
            // Log the exception.
            // _logger.LogError(ex, $"Error occurred while deleting category with ID: {id}.");
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", ["Đã xảy ra lỗi khi xóa danh mục. Vui lòng thử lại sau."] } });
        }
    }
}