using application.Interfaces;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

public partial class ProductFieldDefinitionService : IProductFieldDefinitionService
{
    private readonly ApplicationDbContext _context; // Inject DbContext

    public ProductFieldDefinitionService(ApplicationDbContext context) // Constructor
    {
        _context = context;
    }

    public async Task<List<ProductFieldDefinition>> GetAllAsync()
    {
        try
        {
            // Truy vấn trực tiếp
            return await _context.ProductFieldDefinitions
                .AsNoTracking()
                .Where(c => c.DeletedAt == null)
                .Include(c => c.ProductType)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ProductFieldDefinition?> GetByIdAsync(int id)
    {
        try
        {
            // Truy vấn trực tiếp
            return await _context.ProductFieldDefinitions
                .Where(c => c.Id == id && c.DeletedAt == null)
                .Include(c => c.ProductType)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<ProductFieldDefinition>> GetByProductTypeAsync(int productTypeId)
    {
        try
        {
            // Truy vấn trực tiếp
            return await _context.ProductFieldDefinitions
                .Where(c => c.ProductTypeId == productTypeId && c.DeletedAt == null)
                .Include(c => c.ProductType)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<BaseResponse> AddAsync(ProductFieldDefinition model)
    {
        try
        {
            // Validation and direct add
            var errors = new Dictionary<string, string[]>();

            var existingProductField = await _context.ProductFieldDefinitions
                .FirstOrDefaultAsync(c => c.FieldName == model.FieldName &&
                                          c.ProductTypeId == model.ProductTypeId &&
                                          c.DeletedAt == null);

            if (existingProductField != null)
                errors.Add(nameof(model.FieldName), ["Field name đã tồn tại cho loại nội dung này"]);

            if (errors.Count != 0)
                return new ErrorResponse(errors);

            await _context.ProductFieldDefinitions.AddAsync(model);
            await _context.SaveChangesAsync();

            return new SuccessResponse<ProductFieldDefinition>(model, "Thêm thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            });
        }
    }

    public async Task<BaseResponse> UpdateAsync(int id, ProductFieldDefinition model)
    {
        try
        {
            // Check for duplicates, excluding the current record
            var existingField = await _context.ProductFieldDefinitions
                .FirstOrDefaultAsync(c => c.FieldName == model.FieldName &&
                                          c.ProductTypeId == model.ProductTypeId &&
                                          c.Id != id &&  // Exclude current record
                                          c.DeletedAt == null);

            if (existingField != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.FieldName), ["Field name đã tồn tại cho loại nội dung này"] }
                });

            // Find the existing record
            var existingProductFieldDefinition = await _context.ProductFieldDefinitions
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingProductFieldDefinition == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Product field definition không tồn tại"] }
                });

            // Update fields
            existingProductFieldDefinition.ProductTypeId = model.ProductTypeId;
            existingProductFieldDefinition.FieldName = model.FieldName ?? existingProductFieldDefinition.FieldName;
            existingProductFieldDefinition.FieldType = model.FieldType;
            existingProductFieldDefinition.IsRequired = model.IsRequired;
            existingProductFieldDefinition.FieldOptions =
                model.FieldOptions ?? existingProductFieldDefinition.FieldOptions;

            await _context.SaveChangesAsync();

            return new SuccessResponse<ProductFieldDefinition>(existingProductFieldDefinition, "Cập nhật thành công.");
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
            var productFieldDefinition =
                await _context.ProductFieldDefinitions.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

            if (productFieldDefinition == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Product field definition không tồn tại"] } });

            productFieldDefinition.DeletedAt = DateTime.UtcNow; // Soft delete

            await _context.SaveChangesAsync();

            return new SuccessResponse<ProductFieldDefinition>(productFieldDefinition, "Đã xóa thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", [ex.Message] } });
        }
    }
}