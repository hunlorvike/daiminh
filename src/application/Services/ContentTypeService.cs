using application.Interfaces;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

/// <summary>
/// Service for managing content types.
/// </summary>
public partial class ContentTypeService : IContentTypeService
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentTypeService"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public ContentTypeService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all content types.
    /// </summary>
    /// <returns>A list of content types.</returns>
    public async Task<List<ContentType>> GetAllAsync()
    {
        try
        {
            // Retrieve all content types from the database.
            return await _context.ContentTypes
                .AsNoTracking()
                .Where(x => x.DeletedAt == null)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, "GetAllAsync ContentTypeService");
            throw new Exception("Đã xảy ra lỗi khi lấy danh sách loại nội dung.", ex);
        }
    }

    /// <summary>
    /// Retrieves a content type by its ID.
    /// </summary>
    /// <param name="id">The ID of the content type.</param>
    /// <returns>The content type, or null if not found.</returns>
    public async Task<ContentType?> GetByIdAsync(int id)
    {
        try
        {
            // Retrieve a content type by ID from the database.
            return await _context.ContentTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null); //use FirstOrDefaultAsync direct
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"GetByIdAsync ContentTypeService with id: {id}");
            throw new Exception($"Đã xảy ra lỗi khi lấy thông tin loại nội dung có ID: {id}.", ex);
        }
    }

    /// <summary>
    /// Adds a new content type.
    /// </summary>
    /// <param name="model">The content type to add.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> AddAsync(ContentType model)
    {
        try
        {
            // Check for duplicate slugs.
            var errors = new Dictionary<string, string[]>();

            var existingContentType = await _context.ContentTypes
                .FirstOrDefaultAsync(ct => ct.Slug == model.Slug && ct.DeletedAt == null);

            if (existingContentType != null)
                errors.Add(nameof(model.Slug), ["Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác."]);

            if (errors.Count != 0) return new ErrorResponse(errors);

            // Add the new content type to the database.
            await _context.ContentTypes.AddAsync(model);
            await _context.SaveChangesAsync();

            return new SuccessResponse<ContentType>(model, "Thêm loại nội dung mới thành công.");
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, "AddAsync ContentTypeService");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi thêm loại nội dung mới. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Updates an existing content type.
    /// </summary>
    /// <param name="id">The ID of the content type to update.</param>
    /// <param name="model">The updated content type data.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> UpdateAsync(int id, ContentType model)
    {
        try
        {
            // Check for duplicate slugs (excluding the current record).
            var existingSlug = await _context.ContentTypes
                .FirstOrDefaultAsync(ct => ct.Slug == model.Slug && ct.Id != id && ct.DeletedAt == null);

            if (existingSlug != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Slug), ["Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác."] }
                });

            // Find the existing content type by ID.
            var existingContentType = await _context.ContentTypes
                .FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null);

            if (existingContentType == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Loại nội dung không tồn tại hoặc đã bị xóa."] }
                });

            // Update the content type properties.
            existingContentType.Name = model.Name ?? existingContentType.Name;
            existingContentType.Slug = model.Slug ?? existingContentType.Slug;

            // Save the changes to the database.
            await _context.SaveChangesAsync();

            return new SuccessResponse<ContentType>(existingContentType, "Cập nhật loại nội dung thành công.");
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"UpdateAsync ContentTypeService with id: {id}");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi cập nhật loại nội dung. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Deletes a content type (soft delete).
    /// </summary>
    /// <param name="id">The ID of the content type to delete.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> DeleteAsync(int id)
    {
        try
        {
            // Find the content type by ID (only if not already soft-deleted).
            var contentType = await _context.ContentTypes.FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null);

            if (contentType == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Loại nội dung không tồn tại hoặc đã bị xóa."] } });

            // Perform a soft delete by setting the DeletedAt property.
            contentType.DeletedAt = DateTime.UtcNow; // Soft delete

            await _context.SaveChangesAsync();

            return new SuccessResponse<ContentType>(contentType, "Xóa loại nội dung thành công (đã ẩn).");
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"DeleteAsync ContentTypeService with id: {id}");
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", ["Đã xảy ra lỗi khi xóa loại nội dung. Vui lòng thử lại sau."] } });
        }
    }
}