using application.Interfaces;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

public partial class ContentFieldDefinitionService : IContentFieldDefinitionService
{
    private readonly ApplicationDbContext _context;

    public ContentFieldDefinitionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ContentFieldDefinition>> GetAllAsync()
    {
        try
        {
            return await _context.ContentFieldDefinitions
                .AsNoTracking()
                .Where(c => c.DeletedAt == null)
                .Include(c => c.ContentType)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ContentFieldDefinition?> GetByIdAsync(int id)
    {
        try
        {
            return await _context.ContentFieldDefinitions
                .Where(c => c.Id == id && c.DeletedAt == null)
                .Include(c => c.ContentType)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<ContentFieldDefinition>> GetByContentTypeAsync(int contentTypeId)
    {
        try
        {
            // Truy vấn trực tiếp
            return await _context.ContentFieldDefinitions
                .Where(c => c.ContentTypeId == contentTypeId && c.DeletedAt == null)
                .Include(c => c.ContentType)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<BaseResponse> AddAsync(ContentFieldDefinition model)
    {
        try
        {
            // Validation và thêm trực tiếp
            var errors = new Dictionary<string, string[]>();

            var existingContentField = await _context.ContentFieldDefinitions
                .FirstOrDefaultAsync(c => c.FieldName == model.FieldName &&
                                          c.ContentTypeId == model.ContentTypeId &&
                                          c.DeletedAt == null);

            if (existingContentField != null)
                errors.Add(nameof(model.FieldName), ["Field name đã tồn tại cho loại nội dung này"]);

            if (errors.Count != 0)
                return new ErrorResponse(errors);

            await _context.ContentFieldDefinitions.AddAsync(model);
            await _context.SaveChangesAsync();

            return new SuccessResponse<ContentFieldDefinition>(model, "Thêm thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            });
        }
    }

    public async Task<BaseResponse> UpdateAsync(int id, ContentFieldDefinition model)
    {
        try
        {
            // Validation và cập nhật trực tiếp

            var existingField = await _context.ContentFieldDefinitions
                .FirstOrDefaultAsync(c => c.FieldName == model.FieldName &&
                                          c.ContentTypeId == model.ContentTypeId &&
                                          c.Id != id &&  // Quan trọng: Loại trừ bản ghi hiện tại
                                          c.DeletedAt == null);

            if (existingField != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.FieldName), ["Field name đã tồn tại cho loại nội dung này"] }
                });

            var existingContentFieldDefinition = await _context.ContentFieldDefinitions
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingContentFieldDefinition == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Content field definition không tồn tại"] }
                });
            //Cập nhật fields
            existingContentFieldDefinition.ContentTypeId = model.ContentTypeId;
            existingContentFieldDefinition.FieldName = model.FieldName ?? existingContentFieldDefinition.FieldName;
            existingContentFieldDefinition.FieldType = model.FieldType;
            existingContentFieldDefinition.IsRequired = model.IsRequired;
            existingContentFieldDefinition.FieldOptions =
                model.FieldOptions ?? existingContentFieldDefinition.FieldOptions;

            await _context.SaveChangesAsync();

            return new SuccessResponse<ContentFieldDefinition>(existingContentFieldDefinition, "Cập nhật thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            });
        }
    }

    public async Task<BaseResponse> DeleteAsync(int id)
    {
        try
        {
            // Tìm và soft-delete
            var contentFieldDefinition =
                await _context.ContentFieldDefinitions.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

            if (contentFieldDefinition == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Content field definition không tồn tại"] } });

            contentFieldDefinition.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new SuccessResponse<ContentFieldDefinition>(contentFieldDefinition, "Đã xóa thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", [ex.Message] } });
        }
    }
}