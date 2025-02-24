using core.Attributes;
using Core.Common.Models;
using core.Entities;
using core.Interfaces.Infrastructure;
using core.Interfaces.Service;
using Microsoft.EntityFrameworkCore;

namespace core.Services;

public partial class ContentTypeService(IUnitOfWork unitOfWork) : ScopedService, IContentTypeService
{
    public async Task<List<ContentType>> GetAllAsync()
    {
        try
        {
            var contentTypeRepository = unitOfWork.GetRepository<ContentType, int>();

            return await contentTypeRepository
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ContentType?> GetByIdAsync(int id)
    {
        try
        {
            var contentTypeRepository = unitOfWork.GetRepository<ContentType, int>();

            return await contentTypeRepository
                .Where(ct => ct.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<BaseResponse> AddAsync(ContentType model)
    {
        try
        {
            var contentTypeRepository = unitOfWork.GetRepository<ContentType, int>();

            var errors = new Dictionary<string, string>();

            var existingContentType = await contentTypeRepository
                .FirstOrDefaultAsync(ct => ct.Slug == model.Slug);

            if (existingContentType != null)
                errors.Add(nameof(model.Slug), "Slug đã tồn tại");

            if (errors.Count != 0) return new ErrorResponse(errors);

            await contentTypeRepository.AddAsync(model);
            await unitOfWork.SaveChangesAsync();

            return new SuccessResponse<ContentType>(model, "Thêm thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string>
            {
                { "General", ex.Message }
            });
        }
    }

    public async Task<BaseResponse> UpdateAsync(int id, ContentType model)
    {
        try
        {
            var contentTypeRepository = unitOfWork.GetRepository<ContentType, int>();

            var existingSlug = await contentTypeRepository
                .FirstOrDefaultAsync(ct => ct.Slug == model.Slug && ct.Id != id);

            if (existingSlug != null)
                return new ErrorResponse(new Dictionary<string, string>
                {
                    { nameof(model.Slug), "Slug đã tồn tại" }
                });

            var existingContentType = await contentTypeRepository
                .FirstOrDefaultAsync(ct => ct.Id == id);

            if (existingContentType == null)
                return new ErrorResponse(new Dictionary<string, string>
                {
                    { "General", "ContentType không tồn tại" }
                });

            existingContentType.Name = model.Name ?? existingContentType.Name;
            existingContentType.Slug = model.Slug ?? existingContentType.Slug;

            await unitOfWork.SaveChangesAsync();

            return new SuccessResponse<ContentType>(existingContentType, "Cập nhật thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string>
            {
                { "General", ex.Message }
            });
        }
    }

    public async Task<BaseResponse> DeleteAsync(int id)
    {
        try
        {
            var contentTypeRepository = unitOfWork.GetRepository<ContentType, int>();
            var contentType = await contentTypeRepository.FirstOrDefaultAsync(ct => ct.Id == id);

            if (contentType == null)
                return new ErrorResponse(new Dictionary<string, string>
                    { { "General", "Loại bài viết không tồn tại." } });

            contentType.DeletedAt = DateTime.UtcNow;

            await unitOfWork.SaveChangesAsync();

            return new SuccessResponse<ContentType>(contentType, "Đã xóa thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string> { { "General", ex.Message } });
        }
    }
}