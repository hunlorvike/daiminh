using application.Interfaces;
using domain.Entities;
using Microsoft.EntityFrameworkCore;
using shared.Interfaces;
using shared.Models;

namespace application.Services;

public class TagService(IUnitOfWork unitOfWork) : ITagService
{
    public async Task<List<Tag>> GetAllAsync()
    {
        try
        {
            var tagRepository = unitOfWork.GetRepository<Tag, int>();

            return await tagRepository
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
            var tagRepository = unitOfWork.GetRepository<Tag, int>();

            return await tagRepository
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
            var tagRepository = unitOfWork.GetRepository<Tag, int>();

            var errors = new Dictionary<string, string[]>();

            var existingTag = await tagRepository
                .FirstOrDefaultAsync(t => t.Slug == model.Slug);

            if (existingTag != null)
                errors.Add(nameof(model.Slug), ["Slug đã tồn tại"]);

            if (errors.Count != 0) return new ErrorResponse(errors);

            await tagRepository.AddAsync(model);
            await unitOfWork.SaveChangesAsync();

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
            var tagRepository = unitOfWork.GetRepository<Tag, int>();

            var existingSlug = await tagRepository
                .FirstOrDefaultAsync(t => t.Slug == model.Slug && t.Id != id);

            if (existingSlug != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Slug), ["Slug đã tồn tại"] }
                });

            var existingTag = await tagRepository
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existingTag == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Thẻ không tồn tại"] }
                });

            existingTag.Name = model.Name ?? existingTag.Name;
            existingTag.Slug = model.Slug ?? existingTag.Slug;

            await unitOfWork.SaveChangesAsync();

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
            var tagRepository = unitOfWork.GetRepository<Tag, int>();
            var tag = await tagRepository.FirstOrDefaultAsync(t => t.Id == id);

            if (tag == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Thẻ không tồn tại."] } });

            tag.DeletedAt = DateTime.UtcNow;

            await unitOfWork.SaveChangesAsync();

            return new SuccessResponse<Tag>(tag, "Xóa thẻ thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", [ex.Message] } });
        }
    }
}