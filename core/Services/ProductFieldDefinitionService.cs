using core.Attributes;
using Core.Common.Models;
using core.Entities;
using core.Interfaces.Infrastructure;
using core.Interfaces.Service;
using Microsoft.EntityFrameworkCore;

namespace core.Services;

public partial class ProductFieldDefinitionService(IUnitOfWork unitOfWork) : ScopedService, IProductFieldDefinitionService
{
    public async Task<List<ProductFieldDefinition>> GetAllAsync()
    {
        try
        {
            var productFieldDefinitionRepository = unitOfWork.GetRepository<ProductFieldDefinition, int>();
            
            return await productFieldDefinitionRepository
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
            var productFieldDefinitionRepository = unitOfWork.GetRepository<ProductFieldDefinition, int>();
            return await productFieldDefinitionRepository
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
            var productFieldDefinitionRepository = unitOfWork.GetRepository<ProductFieldDefinition, int>();
            return await productFieldDefinitionRepository
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
            var productFieldDefinitionRepository = unitOfWork.GetRepository<ProductFieldDefinition, int>();
            var errors = new Dictionary<string, string[]>();

            var existingProductField = await productFieldDefinitionRepository
                .FirstOrDefaultAsync(c => c.FieldName == model.FieldName && 
                                          c.ProductTypeId == model.ProductTypeId && 
                                          c.DeletedAt == null);

            if (existingProductField != null)
                errors.Add(nameof(model.FieldName), ["Field name đã tồn tại cho loại nội dung này"]);

            if (errors.Count != 0)
                return new ErrorResponse(errors);

            await productFieldDefinitionRepository.AddAsync(model);
            await unitOfWork.SaveChangesAsync();

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
            var productFieldDefinitionRepository = unitOfWork.GetRepository<ProductFieldDefinition, int>();

            var existingField = await productFieldDefinitionRepository
                .FirstOrDefaultAsync(c => c.FieldName == model.FieldName && 
                                   c.ProductTypeId == model.ProductTypeId && 
                                   c.Id != id && 
                                   c.DeletedAt == null);

            if (existingField != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.FieldName), ["Field name đã tồn tại cho loại nội dung này"] }
                });

            var existingProductFieldDefinition = await productFieldDefinitionRepository
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingProductFieldDefinition == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Product field definition không tồn tại"] }
                });

            existingProductFieldDefinition.ProductTypeId = model.ProductTypeId;
            existingProductFieldDefinition.FieldName = model.FieldName ?? existingProductFieldDefinition.FieldName;
            existingProductFieldDefinition.FieldType = model.FieldType;
            existingProductFieldDefinition.IsRequired = model.IsRequired;
            existingProductFieldDefinition.FieldOptions = model.FieldOptions ?? existingProductFieldDefinition.FieldOptions;

            await unitOfWork.SaveChangesAsync();

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
            var productFieldDefinitionRepository = unitOfWork.GetRepository<ProductFieldDefinition, int>();
            var productFieldDefinition = await productFieldDefinitionRepository.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

            if (productFieldDefinition == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Product field definition không tồn tại"] } });

            productFieldDefinition.DeletedAt = DateTime.UtcNow;

            await unitOfWork.SaveChangesAsync();

            return new SuccessResponse<ProductFieldDefinition>(productFieldDefinition, "Đã xóa thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", [ex.Message] } });
        }
    }
}