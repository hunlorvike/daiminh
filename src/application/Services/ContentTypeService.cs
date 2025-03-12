using application.Interfaces;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

public partial class ContentTypeService : IContentTypeService
{
    private readonly ApplicationDbContext _context;

    public ContentTypeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ContentType>> GetAllAsync()
    {
        try
        {
            // Truy vấn trực tiếp
            return await _context.ContentTypes
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
            // Truy vấn trực tiếp
            return await _context.ContentTypes
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
            // Validation và thêm trực tiếp
            var errors = new Dictionary<string, string[]>();

            var existingContentType = await _context.ContentTypes
                .FirstOrDefaultAsync(ct => ct.Slug == model.Slug);

            if (existingContentType != null)
                errors.Add(nameof(model.Slug), ["Slug đã tồn tại"]);

            if (errors.Count != 0) return new ErrorResponse(errors);

            await _context.ContentTypes.AddAsync(model);
            await _context.SaveChangesAsync();

            return new SuccessResponse<ContentType>(model, "Thêm thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            });
        }
    }

    public async Task<BaseResponse> UpdateAsync(int id, ContentType model)
    {
        try
        {
            // Check for duplicate slug, excluding the current record
            var existingSlug = await _context.ContentTypes
                .FirstOrDefaultAsync(ct => ct.Slug == model.Slug && ct.Id != id);

            if (existingSlug != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Slug), ["Slug đã tồn tại"] }
                });

            // Find the existing record
            var existingContentType = await _context.ContentTypes
                .FirstOrDefaultAsync(ct => ct.Id == id);

            if (existingContentType == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["ContentType không tồn tại"] }
                });
            // Update fields
            existingContentType.Name = model.Name ?? existingContentType.Name;
            existingContentType.Slug = model.Slug ?? existingContentType.Slug;

            await _context.SaveChangesAsync();

            return new SuccessResponse<ContentType>(existingContentType, "Cập nhật thành công.");
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
            // Soft delete
            var contentType = await _context.ContentTypes.FirstOrDefaultAsync(ct => ct.Id == id);

            if (contentType == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Loại bài viết không tồn tại."] } });

            contentType.DeletedAt = DateTime.UtcNow; // Soft delete

            await _context.SaveChangesAsync();

            return new SuccessResponse<ContentType>(contentType, "Đã xóa thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", [ex.Message] } });
        }
    }
}