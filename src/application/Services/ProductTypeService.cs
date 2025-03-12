using application.Interfaces;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

public partial class ProductTypeService : IProductTypeService
{
    private readonly ApplicationDbContext _context; // Inject DbContext

    public ProductTypeService(ApplicationDbContext context) // Constructor
    {
        _context = context;
    }

    public async Task<List<ProductType>> GetAllAsync()
    {
        try
        {
            // Direct query
            return await _context.ProductTypes
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ProductType?> GetByIdAsync(int id)
    {
        try
        {
            // Direct query
            return await _context.ProductTypes
                .Where(ct => ct.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<BaseResponse> AddAsync(ProductType model)
    {
        try
        {
            // Validation and direct add
            var errors = new Dictionary<string, string[]>();

            var existingProductType = await _context.ProductTypes
                .FirstOrDefaultAsync(ct => ct.Slug == model.Slug);

            if (existingProductType != null)
                errors.Add(nameof(model.Slug), ["Slug đã tồn tại"]);

            if (errors.Count != 0) return new ErrorResponse(errors);

            await _context.ProductTypes.AddAsync(model);
            await _context.SaveChangesAsync();

            return new SuccessResponse<ProductType>(model, "Thêm thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            });
        }
    }

    public async Task<BaseResponse> UpdateAsync(int id, ProductType model)
    {
        try
        {
            // Check for duplicate slug, excluding current record
            var existingSlug = await _context.ProductTypes
                .FirstOrDefaultAsync(ct => ct.Slug == model.Slug && ct.Id != id);

            if (existingSlug != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Slug), ["Slug đã tồn tại"] }
                });

            // Find existing record
            var existingProductType = await _context.ProductTypes
                .FirstOrDefaultAsync(ct => ct.Id == id);

            if (existingProductType == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["ProductType không tồn tại"] }
                });
            // Update fields
            existingProductType.Name = model.Name ?? existingProductType.Name;
            existingProductType.Slug = model.Slug ?? existingProductType.Slug;

            await _context.SaveChangesAsync();

            return new SuccessResponse<ProductType>(existingProductType, "Cập nhật thành công.");
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
            var productType = await _context.ProductTypes.FirstOrDefaultAsync(ct => ct.Id == id);

            if (productType == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Loại sản phẩm không tồn tại."] } });

            productType.DeletedAt = DateTime.UtcNow; // Soft delete

            await _context.SaveChangesAsync();

            return new SuccessResponse<ProductType>(productType, "Đã xóa thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", [ex.Message] } });
        }
    }
}