using application.Interfaces;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

/// <summary>
/// Service for managing product field definitions.
/// </summary>
public partial class ProductFieldDefinitionService : IProductFieldDefinitionService
{
    private readonly ApplicationDbContext _context; // Inject DbContext

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductFieldDefinitionService"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public ProductFieldDefinitionService(ApplicationDbContext context) // Constructor
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all product field definitions.
    /// </summary>
    /// <returns>A list of product field definitions.</returns>
    public async Task<List<ProductFieldDefinition>> GetAllAsync()
    {
        try
        {
            // Retrieve all product field definitions, including the related ProductType, that are not soft-deleted.
            return await _context.ProductFieldDefinitions
                .AsNoTracking() // Use AsNoTracking for read-only scenarios
                .Where(c => c.DeletedAt == null)
                .Include(c => c.ProductType)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, "GetAllAsync ProductFieldDefinitionService");
            throw new Exception("Đã xảy ra lỗi khi lấy danh sách định nghĩa trường sản phẩm.", ex);
        }
    }

    /// <summary>
    /// Retrieves a product field definition by its ID.
    /// </summary>
    /// <param name="id">The ID of the product field definition.</param>
    /// <returns>The product field definition, or null if not found.</returns>
    public async Task<ProductFieldDefinition?> GetByIdAsync(int id)
    {
        try
        {
            // Retrieve a product field definition by ID, including the related ProductType.
            return await _context.ProductFieldDefinitions
                .Where(c => c.Id == id && c.DeletedAt == null)
                .Include(c => c.ProductType)
                .AsNoTracking() // Use AsNoTracking for read-only scenarios
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"GetByIdAsync ProductFieldDefinitionService with id: {id}");
            throw new Exception($"Đã xảy ra lỗi khi lấy thông tin định nghĩa trường sản phẩm có ID: {id}.", ex);
        }
    }

    /// <summary>
    /// Retrieves all product field definitions for a specific product type.
    /// </summary>
    /// <param name="productTypeId">The ID of the product type.</param>
    /// <returns>A list of product field definitions.</returns>
    public async Task<List<ProductFieldDefinition>> GetByProductTypeAsync(int productTypeId)
    {
        try
        {
            // Retrieve all product field definitions for a specific product type, including the related ProductType.
            return await _context.ProductFieldDefinitions
                .Where(c => c.ProductTypeId == productTypeId && c.DeletedAt == null)
                .Include(c => c.ProductType)
                .AsNoTracking() // Use AsNoTracking for read-only operations
                .ToListAsync();
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"GetByProductTypeAsync ProductFieldDefinitionService with id: {productTypeId}");
            throw new Exception($"Đã xảy ra lỗi khi lấy danh sách định nghĩa trường sản phẩm cho loại sản phẩm có ID: {productTypeId}.", ex);
        }
    }

    /// <summary>
    /// Adds a new product field definition.
    /// </summary>
    /// <param name="model">The product field definition to add.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> AddAsync(ProductFieldDefinition model)
    {
        try
        {
            // Check for duplicate field names within the same product type.
            var errors = new Dictionary<string, string[]>();

            var existingProductField = await _context.ProductFieldDefinitions
                .FirstOrDefaultAsync(c => c.FieldName == model.FieldName &&
                                          c.ProductTypeId == model.ProductTypeId &&
                                          c.DeletedAt == null);

            if (existingProductField != null)
                errors.Add(nameof(model.FieldName), ["Tên trường đã tồn tại trong loại sản phẩm này. Vui lòng chọn một tên khác."]);

            if (errors.Count != 0)
                return new ErrorResponse(errors);

            // Add the new product field definition to the database.
            await _context.ProductFieldDefinitions.AddAsync(model);
            await _context.SaveChangesAsync();

            return new SuccessResponse<ProductFieldDefinition>(model, "Thêm định nghĩa trường sản phẩm mới thành công.");
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, "AddAsync ProductFieldDefinitionService");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi thêm định nghĩa trường sản phẩm mới. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Updates an existing product field definition.
    /// </summary>
    /// <param name="id">The ID of the product field definition to update.</param>
    /// <param name="model">The updated product field definition data.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> UpdateAsync(int id, ProductFieldDefinition model)
    {
        try
        {
            // Check for duplicate field names within the same product type (excluding the current record).
            var existingField = await _context.ProductFieldDefinitions
                .FirstOrDefaultAsync(c => c.FieldName == model.FieldName &&
                                          c.ProductTypeId == model.ProductTypeId &&
                                          c.Id != id &&  // Exclude current record
                                          c.DeletedAt == null);

            if (existingField != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.FieldName), ["Tên trường đã tồn tại trong loại sản phẩm này. Vui lòng chọn một tên khác."] }
                });

            // Find the existing product field definition by ID.
            var existingProductFieldDefinition = await _context.ProductFieldDefinitions
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null); // Check for DeletedAt also

            if (existingProductFieldDefinition == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Định nghĩa trường sản phẩm không tồn tại hoặc đã bị xóa."] }
                });

            // Update the product field definition properties.
            existingProductFieldDefinition.ProductTypeId = model.ProductTypeId;  // No null check needed
            existingProductFieldDefinition.FieldName = model.FieldName ?? existingProductFieldDefinition.FieldName;
            existingProductFieldDefinition.FieldType = model.FieldType; // No null check (enum with default)
            existingProductFieldDefinition.IsRequired = model.IsRequired; // No null check (bool)
            existingProductFieldDefinition.FieldOptions = model.FieldOptions ?? existingProductFieldDefinition.FieldOptions;

            // Save the changes to the database.
            await _context.SaveChangesAsync();

            return new SuccessResponse<ProductFieldDefinition>(existingProductFieldDefinition, "Cập nhật định nghĩa trường sản phẩm thành công.");
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"UpdateAsync ProductFieldDefinitionService with id: {id}");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi cập nhật định nghĩa trường sản phẩm. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Deletes a product field definition (soft delete).
    /// </summary>
    /// <param name="id">The ID of the product field definition to delete.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> DeleteAsync(int id)
    {
        try
        {
            // Find the product field definition by ID (only if not already soft-deleted).
            var productFieldDefinition =
                await _context.ProductFieldDefinitions.FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

            if (productFieldDefinition == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Định nghĩa trường sản phẩm không tồn tại hoặc đã bị xóa."] } });

            // Perform a soft delete by setting the DeletedAt property.
            productFieldDefinition.DeletedAt = DateTime.UtcNow; // Soft delete

            await _context.SaveChangesAsync();

            return new SuccessResponse<ProductFieldDefinition>(productFieldDefinition, "Xóa định nghĩa trường sản phẩm thành công (đã ẩn).");
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"DeleteAsync ProductFieldDefinitionService with id: {id}");
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", ["Đã xảy ra lỗi khi xóa định nghĩa trường sản phẩm. Vui lòng thử lại sau."] } });
        }
    }
}