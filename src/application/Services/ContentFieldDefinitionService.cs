using application.Interfaces;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

/// <summary>
/// Service for managing content field definitions.
/// </summary>
public partial class ContentFieldDefinitionService : IContentFieldDefinitionService
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentFieldDefinitionService"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public ContentFieldDefinitionService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all content field definitions.
    /// </summary>
    /// <returns>A list of content field definitions.</returns>
    public async Task<List<ContentFieldDefinition>> GetAllAsync()
    {
        try
        {
            // Retrieve all content field definitions that are not soft-deleted, including their related content types.
            return await _context.ContentFieldDefinitions
                .AsNoTracking() // Use AsNoTracking for read-only scenarios
                .Where(c => c.DeletedAt == null)
                .Include(c => c.ContentType)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            // Log exception
            //_logger.LogError(ex, "An error occurred while retrieving all content field definitions.");
            throw new Exception("Đã xảy ra lỗi khi lấy danh sách định nghĩa trường nội dung.", ex);
        }
    }

    /// <summary>
    /// Retrieves a content field definition by its ID.
    /// </summary>
    /// <param name="id">The ID of the content field definition.</param>
    /// <returns>The content field definition, or null if not found.</returns>
    public async Task<ContentFieldDefinition?> GetByIdAsync(int id)
    {
        try
        {
            // Retrieve a content field definition by ID, including its related content type.
            return await _context.ContentFieldDefinitions
                .Where(c => c.Id == id && c.DeletedAt == null)
                .Include(c => c.ContentType)
                .AsNoTracking() // Use AsNoTracking for read-only operations
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            // Log exception
            //_logger.LogError(ex, $"An error occurred while retrieving content field definition with ID: {id}.");
            throw new Exception($"Đã xảy ra lỗi khi lấy thông tin định nghĩa trường nội dung có ID: {id}.", ex);
        }
    }

    /// <summary>
    /// Retrieves all content field definitions for a specific content type.
    /// </summary>
    /// <param name="contentTypeId">The ID of the content type.</param>
    /// <returns>A list of content field definitions.</returns>
    public async Task<List<ContentFieldDefinition>> GetByContentTypeAsync(int contentTypeId)
    {
        try
        {
            // Retrieve all content field definitions for a given content type ID, including their related content types.
            return await _context.ContentFieldDefinitions
                .Where(c => c.ContentTypeId == contentTypeId && c.DeletedAt == null)
                .Include(c => c.ContentType)
                .AsNoTracking() // Use AsNoTracking for read-only operations
                .ToListAsync();
        }
        catch (Exception ex)
        {
            // Log exception
            // _logger.LogError(ex, $"An error occurred while retrieving content field definitions for content type ID: {contentTypeId}.");
            throw new Exception($"Đã xảy ra lỗi khi lấy danh sách định nghĩa trường nội dung cho loại nội dung có ID: {contentTypeId}.", ex);
        }
    }

    /// <summary>
    /// Adds a new content field definition.
    /// </summary>
    /// <param name="model">The content field definition to add.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> AddAsync(ContentFieldDefinition model)
    {
        try
        {
            // Check for duplicate field names within the same content type.
            var errors = new Dictionary<string, string[]>();

            var existingContentField = await _context.ContentFieldDefinitions
                .FirstOrDefaultAsync(c => c.FieldName == model.FieldName &&
                                          c.ContentTypeId == model.ContentTypeId &&
                                          c.DeletedAt == null);

            if (existingContentField != null)
                errors.Add(nameof(model.FieldName), ["Tên trường đã tồn tại trong loại nội dung này. Vui lòng chọn một tên khác."]);

            if (errors.Count != 0)
                return new ErrorResponse(errors);

            // Add the new content field definition to the database.
            await _context.ContentFieldDefinitions.AddAsync(model);
            await _context.SaveChangesAsync();

            return new SuccessResponse<ContentFieldDefinition>(model, "Thêm định nghĩa trường nội dung mới thành công.");
        }
        catch (Exception ex)
        {
            // Log exception
            // _logger.LogError(ex, "An error occurred while adding a new content field definition.");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi thêm định nghĩa trường nội dung mới. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Updates an existing content field definition.
    /// </summary>
    /// <param name="id">The ID of the content field definition to update.</param>
    /// <param name="model">The updated content field definition data.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> UpdateAsync(int id, ContentFieldDefinition model)
    {
        try
        {
            // Check for duplicate field names within the same content type (excluding the current record).
            var existingField = await _context.ContentFieldDefinitions
                .FirstOrDefaultAsync(c => c.FieldName == model.FieldName &&
                                          c.ContentTypeId == model.ContentTypeId &&
                                          c.Id != id &&  // Exclude the current record
                                          c.DeletedAt == null);

            if (existingField != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.FieldName), ["Tên trường đã tồn tại trong loại nội dung này. Vui lòng chọn một tên khác."] }
                });

            // Find the existing content field definition by ID.
            var existingContentFieldDefinition = await _context.ContentFieldDefinitions
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null); // Also check DeletedAt here

            if (existingContentFieldDefinition == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Định nghĩa trường nội dung không tồn tại hoặc đã bị xóa."] }
                });

            // Update the content field definition properties.
            existingContentFieldDefinition.ContentTypeId = model.ContentTypeId; // No null check needed
            existingContentFieldDefinition.FieldName = model.FieldName ?? existingContentFieldDefinition.FieldName;
            existingContentFieldDefinition.FieldType = model.FieldType; // No null check needed (enum with default)
            existingContentFieldDefinition.IsRequired = model.IsRequired; // No null check needed (bool)
            existingContentFieldDefinition.FieldOptions = model.FieldOptions ?? existingContentFieldDefinition.FieldOptions;

            // Save the changes to the database.
            await _context.SaveChangesAsync();

            return new SuccessResponse<ContentFieldDefinition>(existingContentFieldDefinition, "Cập nhật định nghĩa trường nội dung thành công.");
        }
        catch (Exception ex)
        {
            // Log exception
            // _logger.LogError(ex, $"An error occurred while updating content field definition with ID: {id}.");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi cập nhật định nghĩa trường nội dung. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Deletes a content field definition (soft delete).
    /// </summary>
    /// <param name="id">The ID of the content field definition to delete.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> DeleteAsync(int id)
    {
        try
        {
            // Find the content field definition by ID (only if not already soft-deleted).
            var contentFieldDefinition =
                await _context.ContentFieldDefinitions.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

            if (contentFieldDefinition == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Định nghĩa trường nội dung không tồn tại hoặc đã bị xóa."] } });

            // Perform a soft delete by setting the DeletedAt property.
            contentFieldDefinition.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new SuccessResponse<ContentFieldDefinition>(contentFieldDefinition, "Xóa định nghĩa trường nội dung thành công (đã ẩn).");
        }
        catch (Exception ex)
        {
            // Log exception
            // _logger.LogError(ex, $"An error occurred while deleting content field definition with ID: {id}.");
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", ["Đã xảy ra lỗi khi xóa định nghĩa trường nội dung. Vui lòng thử lại sau."] } });
        }
    }
}