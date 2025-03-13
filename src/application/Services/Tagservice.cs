using application.Interfaces;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

/// <summary>
/// Service for managing tags.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TagService"/> class.
/// </remarks>
/// <param name="context">The application database context.</param>
public class TagService(ApplicationDbContext context) : ITagService
{

    /// <summary>
    /// Retrieves all tags.
    /// </summary>
    /// <returns>A list of tags.</returns>
    public async Task<List<Tag>> GetAllAsync()
    {
        try
        {
            // Retrieve all tags from the database.
            return await context.Tags
                .AsNoTracking()
                .Where(x => x.DeletedAt == null)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, "GetAllAsync TagService");
            throw new Exception("Đã xảy ra lỗi khi lấy danh sách thẻ.", ex);
        }
    }

    /// <summary>
    /// Retrieves a tag by its ID.
    /// </summary>
    /// <param name="id">The ID of the tag.</param>
    /// <returns>The tag, or null if not found.</returns>
    public async Task<Tag?> GetByIdAsync(int id)
    {
        try
        {
            // Retrieve a tag by ID from the database.
            return await context.Tags
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && t.DeletedAt == null); // Use FirstOrDefaultAsync
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"GetByIdAsync TagService with id: {id}");
            throw new Exception($"Đã xảy ra lỗi khi lấy thông tin thẻ có ID: {id}.", ex);
        }
    }

    /// <summary>
    /// Adds a new tag.
    /// </summary>
    /// <param name="model">The tag to add.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> AddAsync(Tag model)
    {
        try
        {
            // Check for duplicate slugs.
            var errors = new Dictionary<string, string[]>();

            var existingTag = await context.Tags
                .FirstOrDefaultAsync(t => t.Slug == model.Slug && t.DeletedAt == null);

            if (existingTag != null)
                errors.Add(nameof(model.Slug), ["Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác."]);

            if (errors.Count != 0) return new ErrorResponse(errors);

            // Add the new tag to the database.
            await context.Tags.AddAsync(model);
            await context.SaveChangesAsync();

            return new SuccessResponse<Tag>(model, "Thêm thẻ mới thành công.");
        }
        catch (Exception)
        {
            //log
            //_logger.LogError(ex, "AddAsync TagService");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi thêm thẻ mới. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Updates an existing tag.
    /// </summary>
    /// <param name="id">The ID of the tag to update.</param>
    /// <param name="model">The updated tag data.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> UpdateAsync(int id, Tag model)
    {
        try
        {
            // Check for duplicate slugs (excluding the current record).
            var existingSlug = await context.Tags
                .FirstOrDefaultAsync(t => t.Slug == model.Slug && t.Id != id && t.DeletedAt == null);

            if (existingSlug != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Slug), ["Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác."] }
                });

            // Find the existing tag by ID.
            var existingTag = await context.Tags
                .FirstOrDefaultAsync(t => t.Id == id && t.DeletedAt == null);

            if (existingTag == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Thẻ không tồn tại hoặc đã bị xóa."] }
                });

            // Update the tag properties.
            existingTag.Name = model.Name ?? existingTag.Name;
            existingTag.Slug = model.Slug ?? existingTag.Slug;

            // Save the changes to the database.
            await context.SaveChangesAsync();

            return new SuccessResponse<Tag>(existingTag, "Cập nhật thẻ thành công.");
        }
        catch (Exception)
        {
            //log
            //_logger.LogError(ex, $"UpdateAsync TagService with id: {id}");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi cập nhật thẻ. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Deletes a tag (soft delete).
    /// </summary>
    /// <param name="id">The ID of the tag to delete.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> DeleteAsync(int id)
    {
        try
        {
            // Find the tag by ID (only if not already soft-deleted).
            var tag = await context.Tags.FirstOrDefaultAsync(t => t.Id == id && t.DeletedAt == null);

            if (tag == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Thẻ không tồn tại hoặc đã bị xóa."] } });

            // Perform a soft delete by setting the DeletedAt property.
            tag.DeletedAt = DateTime.UtcNow; // Soft delete

            await context.SaveChangesAsync();

            return new SuccessResponse<Tag>(tag, "Xóa thẻ thành công (đã ẩn).");
        }
        catch (Exception)
        {
            //log
            //_logger.LogError(ex, $"DeleteAsync TagService with id: {id}");
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", ["Đã xảy ra lỗi khi xóa thẻ. Vui lòng thử lại sau."] } });
        }
    }
}