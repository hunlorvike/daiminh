using core.Attributes;
using Core.Common.Models;
using core.Entities;
using core.Interfaces.Infrastructure;
using core.Interfaces.Service;
using Microsoft.EntityFrameworkCore;

namespace core.Services;

public partial class ContentFieldDefinitionService(IUnitOfWork unitOfWork) : ScopedService, IContentFieldDefinitionService
{
    public async Task<List<ContentFieldDefinition>> GetAllAsync()
    {
        try
        {
            var contentFieldDefinitionRepository = unitOfWork.GetRepository<ContentFieldDefinition, int>();
            
            return await contentFieldDefinitionRepository
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
            var contentFieldDefinitionRepository = unitOfWork.GetRepository<ContentFieldDefinition, int>();
            return await contentFieldDefinitionRepository
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
            var contentFieldDefinitionRepository = unitOfWork.GetRepository<ContentFieldDefinition, int>();
            return await contentFieldDefinitionRepository
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
            var contentFieldDefinitionRepository = unitOfWork.GetRepository<ContentFieldDefinition, int>();
            var errors = new Dictionary<string, string[]>();

            var existingContentField = await contentFieldDefinitionRepository
                .FirstOrDefaultAsync(c => c.FieldName == model.FieldName && 
                                    c.ContentTypeId == model.ContentTypeId && 
                                    c.DeletedAt == null);

            if (existingContentField != null)
                errors.Add(nameof(model.FieldName), ["Field name đã tồn tại cho loại nội dung này"]);

            if (errors.Count != 0)
                return new ErrorResponse(errors);

            await contentFieldDefinitionRepository.AddAsync(model);
            await unitOfWork.SaveChangesAsync();

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
            var contentFieldDefinitionRepository = unitOfWork.GetRepository<ContentFieldDefinition, int>();

            var existingField = await contentFieldDefinitionRepository
                .FirstOrDefaultAsync(c => c.FieldName == model.FieldName && 
                                   c.ContentTypeId == model.ContentTypeId && 
                                   c.Id != id && 
                                   c.DeletedAt == null);

            if (existingField != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.FieldName), ["Field name đã tồn tại cho loại nội dung này"] }
                });

            var existingContentFieldDefinition = await contentFieldDefinitionRepository
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingContentFieldDefinition == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Content field definition không tồn tại"] }
                });

            existingContentFieldDefinition.ContentTypeId = model.ContentTypeId;
            existingContentFieldDefinition.FieldName = model.FieldName ?? existingContentFieldDefinition.FieldName;
            existingContentFieldDefinition.FieldType = model.FieldType;
            existingContentFieldDefinition.IsRequired = model.IsRequired;
            existingContentFieldDefinition.FieldOptions = model.FieldOptions ?? existingContentFieldDefinition.FieldOptions;

            await unitOfWork.SaveChangesAsync();

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
            var contentFieldDefinitionRepository = unitOfWork.GetRepository<ContentFieldDefinition, int>();
            var contentFieldDefinition = await contentFieldDefinitionRepository.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

            if (contentFieldDefinition == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Content field definition không tồn tại"] } });

            contentFieldDefinition.DeletedAt = DateTime.UtcNow;

            await unitOfWork.SaveChangesAsync();

            return new SuccessResponse<ContentFieldDefinition>(contentFieldDefinition, "Đã xóa thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", [ex.Message] } });
        }
    }
}