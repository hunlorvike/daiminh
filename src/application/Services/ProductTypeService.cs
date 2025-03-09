using application.Interfaces;
using domain.Entities;
using Microsoft.EntityFrameworkCore;
using shared.Attributes;
using shared.Interfaces;
using shared.Models;

namespace application.Services;

public partial class ProductTypeService(IUnitOfWork unitOfWork) : ScopedService, IProductTypeService
{
    public async Task<List<ProductType>> GetAllAsync()
    {
        try
        {
            var productTypeRepository = unitOfWork.GetRepository<ProductType, int>();

            return await productTypeRepository
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
            var productTypeRepository = unitOfWork.GetRepository<ProductType, int>();

            return await productTypeRepository
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
            var productTypeRepository = unitOfWork.GetRepository<ProductType, int>();

            var errors = new Dictionary<string, string[]>();

            var existingProductType = await productTypeRepository
                .FirstOrDefaultAsync(ct => ct.Slug == model.Slug);

            if (existingProductType != null)
                errors.Add(nameof(model.Slug), ["Slug đã tồn tại"]);

            if (errors.Count != 0) return new ErrorResponse(errors);

            await productTypeRepository.AddAsync(model);
            await unitOfWork.SaveChangesAsync();

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
            var productTypeRepository = unitOfWork.GetRepository<ProductType, int>();

            var existingSlug = await productTypeRepository
                .FirstOrDefaultAsync(ct => ct.Slug == model.Slug && ct.Id != id);

            if (existingSlug != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Slug), ["Slug đã tồn tại"] }
                });

            var existingProductType = await productTypeRepository
                .FirstOrDefaultAsync(ct => ct.Id == id);

            if (existingProductType == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["ProductType không tồn tại"] }
                });

            existingProductType.Name = model.Name ?? existingProductType.Name;
            existingProductType.Slug = model.Slug ?? existingProductType.Slug;

            await unitOfWork.SaveChangesAsync();

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
            var productTypeRepository = unitOfWork.GetRepository<ProductType, int>();
            var productType = await productTypeRepository.FirstOrDefaultAsync(ct => ct.Id == id);

            if (productType == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Loại sản phẩm không tồn tại."] } });

            productType.DeletedAt = DateTime.UtcNow;

            await unitOfWork.SaveChangesAsync();

            return new SuccessResponse<ProductType>(productType, "Đã xóa thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", [ex.Message] } });
        }
    }
}