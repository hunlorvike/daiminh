using application.Interfaces;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

/// <summary>
/// Service for managing product types.
/// </summary>
public partial class ProductTypeService : IProductTypeService
{
    private readonly ApplicationDbContext _context; // Inject DbContext

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductTypeService"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public ProductTypeService(ApplicationDbContext context) // Constructor
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all product types.
    /// </summary>
    /// <returns>A list of product types.</returns>
    public async Task<List<ProductType>> GetAllAsync()
    {
        try
        {
            // Retrieve all product types from the database.
            return await _context.ProductTypes
                .AsNoTracking()
                .Where(x => x.DeletedAt == null)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, "GetAllAsync ProductTypeService");
            throw new Exception("Đã xảy ra lỗi khi lấy danh sách loại sản phẩm.", ex);
        }
    }

    /// <summary>
    /// Retrieves a product type by its ID.
    /// </summary>
    /// <param name="id">The ID of the product type.</param>
    /// <returns>The product type, or null if not found.</returns>
    public async Task<ProductType?> GetByIdAsync(int id)
    {
        try
        {
            // Retrieve a product type by ID from the database.
            return await _context.ProductTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null); // Use FirstOrDefaultAsync
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"GetByIdAsync ProductTypeService with id: {id}");
            throw new Exception($"Đã xảy ra lỗi khi lấy thông tin loại sản phẩm có ID: {id}.", ex);
        }
    }

    /// <summary>
    /// Adds a new product type.
    /// </summary>
    /// <param name="model">The product type to add.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> AddAsync(ProductType model)
    {
        try
        {
            // Check for duplicate slugs.
            var errors = new Dictionary<string, string[]>();

            var existingProductType = await _context.ProductTypes
                .FirstOrDefaultAsync(ct => ct.Slug == model.Slug && ct.DeletedAt == null);

            if (existingProductType != null)
                errors.Add(nameof(model.Slug), ["Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác."]);

            if (errors.Count != 0) return new ErrorResponse(errors);

            // Add the new product type to the database.
            await _context.ProductTypes.AddAsync(model);
            await _context.SaveChangesAsync();

            return new SuccessResponse<ProductType>(model, "Thêm loại sản phẩm mới thành công.");
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, "AddAsync ProductTypeService");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi thêm loại sản phẩm mới. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Updates an existing product type.
    /// </summary>
    /// <param name="id">The ID of the product type to update.</param>
    /// <param name="model">The updated product type data.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> UpdateAsync(int id, ProductType model)
    {
        try
        {
            // Check for duplicate slugs (excluding the current record).
            var existingSlug = await _context.ProductTypes
                .FirstOrDefaultAsync(ct => ct.Slug == model.Slug && ct.Id != id && ct.DeletedAt == null);

            if (existingSlug != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Slug), ["Đường dẫn (slug) đã tồn tại. Vui lòng chọn một đường dẫn khác."] }
                });

            // Find the existing product type by ID.
            var existingProductType = await _context.ProductTypes
                .FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null); // Check for DeletedAt

            if (existingProductType == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Loại sản phẩm không tồn tại hoặc đã bị xóa."] }
                });

            // Update the product type properties.
            existingProductType.Name = model.Name ?? existingProductType.Name;
            existingProductType.Slug = model.Slug ?? existingProductType.Slug;

            // Save the changes to the database.
            await _context.SaveChangesAsync();

            return new SuccessResponse<ProductType>(existingProductType, "Cập nhật loại sản phẩm thành công.");
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"UpdateAsync ProductTypeService with id: {id}");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi cập nhật loại sản phẩm. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Deletes a product type (soft delete).
    /// </summary>
    /// <param name="id">The ID of the product type to delete.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> DeleteAsync(int id)
    {
        try
        {
            // Find the product type by ID (only if not already soft-deleted).
            var productType = await _context.ProductTypes.FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null);

            if (productType == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Loại sản phẩm không tồn tại hoặc đã bị xóa."] } });

            // Perform a soft delete by setting the DeletedAt property.
            productType.DeletedAt = DateTime.UtcNow; // Soft delete

            await _context.SaveChangesAsync();

            return new SuccessResponse<ProductType>(productType, "Xóa loại sản phẩm thành công (đã ẩn).");
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"DeleteAsync ProductTypeService with id: {id}");
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", ["Đã xảy ra lỗi khi xóa loại sản phẩm. Vui lòng thử lại sau."] } });
        }
    }
}