using application.Interfaces;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

public class TagService : ITagService
{
    private readonly ApplicationDbContext _context; // Inject DbContext

    public TagService(ApplicationDbContext context) // Constructor
    {
        _context = context;
    }

    public async Task<List<Tag>> GetAllAsync()
    {
        try
        {
            // Direct query
            return await _context.Tags
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Tag?> GetByIdAsync(int id)
    {
        try
        {
            // Direct query
            return await _context.Tags
                .Where(t => t.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<BaseResponse> AddAsync(Tag model)
    {
        try
        {
            // Validation and direct add
            var errors = new Dictionary<string, string[]>();

            var existingTag = await _context.Tags
                .FirstOrDefaultAsync(t => t.Slug == model.Slug);

            if (existingTag != null)
                errors.Add(nameof(model.Slug), ["Slug đã tồn tại"]);

            if (errors.Count != 0) return new ErrorResponse(errors);

            await _context.Tags.AddAsync(model);
            await _context.SaveChangesAsync();

            return new SuccessResponse<Tag>(model, "Thêm thẻ thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            });
        }
    }

    public async Task<BaseResponse> UpdateAsync(int id, Tag model)
    {
        try
        {
            // Check for duplicate slug (excluding current record)
            var existingSlug = await _context.Tags
                .FirstOrDefaultAsync(t => t.Slug == model.Slug && t.Id != id);

            if (existingSlug != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Slug), ["Slug đã tồn tại"] }
                });

            // Find existing tag
            var existingTag = await _context.Tags
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existingTag == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Thẻ không tồn tại"] }
                });

            // Update fields
            existingTag.Name = model.Name ?? existingTag.Name;
            existingTag.Slug = model.Slug ?? existingTag.Slug;

            await _context.SaveChangesAsync();

            return new SuccessResponse<Tag>(existingTag, "Cập nhật thẻ thành công.");
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
            // Find and soft-delete
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (tag == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Thẻ không tồn tại."] } });

            tag.DeletedAt = DateTime.UtcNow; // Soft delete

            await _context.SaveChangesAsync();

            return new SuccessResponse<Tag>(tag, "Xóa thẻ thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", [ex.Message] } });
        }
    }
}