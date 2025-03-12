using application.Interfaces;
using domain.Entities;
using Microsoft.EntityFrameworkCore;
using shared.Interfaces;
using shared.Models;

namespace application.Services;

public partial class CategoryService(IUnitOfWork unitOfWork) : ICategoryService
{
    public async Task<List<Category>> GetAllAsync()
    {
        try
        {
            var categoryRepository = unitOfWork.GetRepository<Category, int>();
            return await categoryRepository
                .AsNoTracking()
                .Where(c => c.DeletedAt == null)
                .Include(c => c.ParentCategory)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        try
        {
            var categoryRepository = unitOfWork.GetRepository<Category, int>();
            return await categoryRepository
                .Where(c => c.Id == id && c.DeletedAt == null)
                .Include(c => c.ParentCategory)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<BaseResponse> AddAsync(Category model)
    {
        try
        {
            var categoryRepository = unitOfWork.GetRepository<Category, int>();
            var errors = new Dictionary<string, string[]>();

            var existingCategory = await categoryRepository
                .FirstOrDefaultAsync(c => c.Slug == model.Slug && c.DeletedAt == null);

            if (existingCategory != null)
                errors.Add(nameof(model.Slug), ["Slug đã tồn tại"]);

            if (errors.Count != 0)
                return new ErrorResponse(errors);

            await categoryRepository.AddAsync(model);
            await unitOfWork.SaveChangesAsync();

            return new SuccessResponse<Category>(model, "Thêm thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            });
        }
    }

    public async Task<BaseResponse> UpdateAsync(int id, Category model)
    {
        try
        {
            var categoryRepository = unitOfWork.GetRepository<Category, int>();

            // Check for duplicate slug on another record
            var existingSlug = await categoryRepository
                .FirstOrDefaultAsync(ct => ct.Slug == model.Slug && ct.Id != id);

            if (existingSlug != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Slug), ["Slug đã tồn tại"] }
                });

            var existingCategory = await categoryRepository
                .FirstOrDefaultAsync(ct => ct.Id == id);

            if (existingCategory == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Category không tồn tại"] }
                });

            existingCategory.Name = model.Name ?? existingCategory.Name;
            existingCategory.Slug = model.Slug ?? existingCategory.Slug;
            existingCategory.ParentCategoryId = model.ParentCategoryId;

            await unitOfWork.SaveChangesAsync();

            return new SuccessResponse<Category>(existingCategory, "Cập nhật thành công.");
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
            var categoryRepository = unitOfWork.GetRepository<Category, int>();
            var category = await categoryRepository.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

            if (category == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Category không tồn tại"] } });

            category.DeletedAt = DateTime.UtcNow;

            await unitOfWork.SaveChangesAsync();

            return new SuccessResponse<Category>(category, "Đã xóa thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", [ex.Message] } });
        }
    }
}