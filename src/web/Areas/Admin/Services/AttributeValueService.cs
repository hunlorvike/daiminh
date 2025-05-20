using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoRegister;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public class AttributeValueService : IAttributeValueService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<AttributeValueService> _logger;

    public AttributeValueService(ApplicationDbContext context, IMapper mapper, ILogger<AttributeValueService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IPagedList<AttributeValueListItemViewModel>> GetPagedAttributeValuesAsync(AttributeValueFilterViewModel filter, int pageNumber, int pageSize)
    {
        IQueryable<AttributeValue> query = _context.Set<AttributeValue>()
                                           .Include(av => av.Attribute)
                                           .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(av => av.Value.ToLower().Contains(lowerSearchTerm) ||
                                      av.Slug != null && av.Slug.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.AttributeId.HasValue && filter.AttributeId.Value > 0)
        {
            query = query.Where(av => av.AttributeId == filter.AttributeId.Value);
        }

        query = query.OrderBy(av => av.Attribute!.Name)
                 .ThenBy(av => av.Value);

        var valuesPaged = await query.ProjectTo<AttributeValueListItemViewModel>(_mapper.ConfigurationProvider)
                                 .ToPagedListAsync(pageNumber, pageSize);

        return valuesPaged;
    }

    public async Task<AttributeValueViewModel?> GetAttributeValueByIdAsync(int id)
    {
        AttributeValue? attributeValue = await _context.Set<AttributeValue>()
                                                 .Include(av => av.Attribute)
                                                 .AsNoTracking()
                                                 .FirstOrDefaultAsync(av => av.Id == id);

        if (attributeValue == null) return null;

        return _mapper.Map<AttributeValueViewModel>(attributeValue);
    }

    public async Task<OperationResult<int>> CreateAttributeValueAsync(AttributeValueViewModel viewModel)
    {
        var parentAttributeExists = await _context.Set<domain.Entities.Attribute>().AnyAsync(a => a.Id == viewModel.AttributeId);
        if (!parentAttributeExists)
        {
            return OperationResult<int>.FailureResult(message: "Thuộc tính cha không tồn tại.", errors: new List<string> { "Thuộc tính cha không tồn tại." });
        }

        if (await IsSlugUniqueAsync(viewModel.Slug!, viewModel.AttributeId))
        {
            return OperationResult<int>.FailureResult(message: "Slug này đã tồn tại cho thuộc tính cha này.", errors: new List<string> { "Slug này đã tồn tại cho thuộc tính cha này." });
        }

        var attributeValue = _mapper.Map<AttributeValue>(viewModel);
        _context.Add(attributeValue);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created AttributeValue: ID={Id}, Value={Value}, AttributeId={AttributeId}", attributeValue.Id, attributeValue.Value, attributeValue.AttributeId);
            return OperationResult<int>.SuccessResult(attributeValue.Id, $"Thêm giá trị '{attributeValue.Value}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi tạo giá trị thuộc tính: Value={Value}, AttributeId={AttributeId}", viewModel.Value, viewModel.AttributeId);
            if (ex.InnerException?.Message?.Contains("UQ_AttributeValue_Slug_AttributeId", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult<int>.FailureResult(message: "Slug này đã tồn tại cho thuộc tính cha này.", errors: new List<string> { "Slug này đã tồn tại cho thuộc tính cha này." });
            }
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi cơ sở dữ liệu khi lưu.", errors: new List<string> { "Đã xảy ra lỗi cơ sở dữ liệu khi lưu." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi tạo giá trị thuộc tính.");
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi không mong muốn khi lưu.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi lưu." });
        }
    }

    public async Task<OperationResult> UpdateAttributeValueAsync(AttributeValueViewModel viewModel)
    {
        var parentAttributeExists = await _context.Set<domain.Entities.Attribute>().AnyAsync(a => a.Id == viewModel.AttributeId);
        if (!parentAttributeExists)
        {
            return OperationResult.FailureResult(message: "Thuộc tính cha không tồn tại.", errors: new List<string> { "Thuộc tính cha không tồn tại." });
        }

        if (await IsSlugUniqueAsync(viewModel.Slug!, viewModel.AttributeId, viewModel.Id)) // Slug uniqueness scoped by parent attribute, ignoring current value
        {
            return OperationResult.FailureResult(message: "Slug đã tồn tại.", errors: new List<string> { "Slug đã tồn tại." });
        }

        var attributeValue = await _context.Set<AttributeValue>().FirstOrDefaultAsync(av => av.Id == viewModel.Id);
        if (attributeValue == null)
        {
            _logger.LogWarning("AttributeValue not found for update. ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult("Không tìm thấy giá trị thuộc tính để cập nhật.");
        }

        _mapper.Map(viewModel, attributeValue);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated AttributeValue: ID={Id}, Value={Value}, AttributeId={AttributeId}", attributeValue.Id, attributeValue.Value, attributeValue.AttributeId);
            return OperationResult.SuccessResult($"Cập nhật giá trị '{attributeValue.Value}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi khi cập nhật giá trị thuộc tính ID {Id}", viewModel.Id);
            if (ex.InnerException?.Message?.Contains("UQ_AttributeValue_Slug_AttributeId", StringComparison.OrdinalIgnoreCase) == true) // Example unique constraint name
            {
                return OperationResult.FailureResult(message: "Slug đã tồn tại.", errors: new List<string> { "Slug đã tồn tại." });
            }
            return OperationResult.FailureResult(message: "Lỗi cơ sở dữ liệu khi cập nhật.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi cập nhật." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật giá trị thuộc tính ID {Id}", viewModel.Id);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi không mong muốn.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn." });
        }
    }

    public async Task<OperationResult<int>> DeleteAttributeValueAsync(int id)
    {
        var attributeValue = await _context.Set<AttributeValue>().FirstOrDefaultAsync(av => av.Id == id);
        if (attributeValue == null)
        {
            _logger.LogWarning("AttributeValue not found for delete. ID: {Id}", id);
            return OperationResult<int>.FailureResult("Không tìm thấy giá trị thuộc tính.");
        }

        int parentAttributeId = attributeValue.AttributeId;
        string valueName = attributeValue.Value;


        if (await IsUsedInProductVariationsAsync(id))
        {
            _logger.LogWarning("Cannot delete AttributeValue {Value} (ID: {Id}) because it's used in product variations.", valueName, id);
            return OperationResult<int>.FailureResult("Không thể xóa giá trị này vì đang được sử dụng trong sản phẩm.");
        }


        _context.Remove(attributeValue);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted AttributeValue: ID={Id}, Value={Value}, ParentAttributeId={ParentId}", id, valueName, parentAttributeId);
            return OperationResult<int>.SuccessResult(parentAttributeId, $"Xóa giá trị '{valueName}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi FK khi xóa giá trị thuộc tính ID {Id}", id);
            if (ex.InnerException?.Message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult<int>.FailureResult("Không thể xóa giá trị này vì đang được sử dụng trong sản phẩm.", errors: new List<string> { "Không thể xóa giá trị này vì đang được sử dụng trong sản phẩm." });
            }
            return OperationResult<int>.FailureResult("Lỗi cơ sở dữ liệu khi xóa giá trị.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi xóa giá trị." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi xóa giá trị thuộc tính ID {Id}", id);
            return OperationResult<int>.FailureResult("Đã xảy ra lỗi không mong muốn khi xóa.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi xóa." });
        }
    }

    public async Task<bool> IsSlugUniqueAsync(string slug, int attributeId, int? ignoreId = null)
    {
        if (string.IsNullOrWhiteSpace(slug)) return false;

        var lowerSlug = slug.Trim().ToLower();
        var query = _context.Set<AttributeValue>()
                            .Where(av => av.Slug.ToLower() == lowerSlug && av.AttributeId == attributeId);

        if (ignoreId.HasValue && ignoreId.Value > 0)
        {
            query = query.Where(av => av.Id != ignoreId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<bool> IsUsedInProductVariationsAsync(int attributeValueId)
    {
        return await _context.Set<ProductVariationAttributeValue>().AnyAsync(pvav => pvav.AttributeValueId == attributeValueId);
    }

    public async Task<AttributeValue?> GetAttributeValueEntityWithParentAsync(int id)
    {
        return await _context.Set<AttributeValue>()
            .Include(av => av.Attribute)
            .AsNoTracking()
            .FirstOrDefaultAsync(av => av.Id == id);
    }
}
