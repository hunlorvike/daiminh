using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Services;

public class AttributeService : IAttributeService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<AttributeService> _logger;

    public AttributeService(ApplicationDbContext context, IMapper mapper, ILogger<AttributeService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IPagedList<AttributeListItemViewModel>> GetPagedAttributesAsync(AttributeFilterViewModel filter, int pageNumber, int pageSize)
    {
        IQueryable<domain.Entities.Attribute> query = _context.Set<domain.Entities.Attribute>()
                                                   .Include(a => a.Values)
                                                   .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(a => a.Name.ToLower().Contains(lowerSearchTerm) ||
                                     a.Slug.ToLower().Contains(lowerSearchTerm));
        }

        query = query.OrderBy(a => a.Name);

        var attributesPaged = await query.ProjectTo<AttributeListItemViewModel>(_mapper.ConfigurationProvider)
                                     .ToPagedListAsync(pageNumber, pageSize);

        return attributesPaged;
    }

    public async Task<AttributeViewModel?> GetAttributeByIdAsync(int id)
    {
        domain.Entities.Attribute? attribute = await _context.Set<domain.Entities.Attribute>()
                                                 .AsNoTracking()
                                                 .FirstOrDefaultAsync(a => a.Id == id);

        return _mapper.Map<AttributeViewModel>(attribute);
    }

    public async Task<OperationResult<int>> CreateAttributeAsync(AttributeViewModel viewModel)
    {
        if (await IsSlugUniqueAsync(viewModel.Slug!))
        {
            return OperationResult<int>.FailureResult(message: "Slug này đã tồn tại.", errors: new List<string> { "Slug này đã tồn tại." });
        }

        var attribute = _mapper.Map<domain.Entities.Attribute>(viewModel);
        _context.Add(attribute);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created Attribute: ID={Id}, Name={Name}", attribute.Id, attribute.Name);
            return OperationResult<int>.SuccessResult(attribute.Id, $"Thêm thuộc tính '{attribute.Name}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi tạo thuộc tính: {Name}", viewModel.Name);
            if (ex.InnerException?.Message?.Contains("UQ_Attribute_Slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult<int>.FailureResult(message: "Slug này đã tồn tại.", errors: new List<string> { "Slug này đã tồn tại." });
            }
            return OperationResult<int>.FailureResult(message: "Lỗi cơ sở dữ liệu khi lưu thuộc tính.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi lưu thuộc tính." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi tạo thuộc tính.");
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi không mong muốn.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn." });
        }
    }

    public async Task<OperationResult> UpdateAttributeAsync(AttributeViewModel viewModel)
    {
        if (await IsSlugUniqueAsync(viewModel.Slug!, viewModel.Id))
        {
            return OperationResult.FailureResult(message: "Slug này đã được sử dụng.", errors: new List<string> { "Slug này đã được sử dụng." });
        }

        var attribute = await _context.Set<domain.Entities.Attribute>().FirstOrDefaultAsync(a => a.Id == viewModel.Id);
        if (attribute == null)
        {
            _logger.LogWarning("Attribute not found for update. ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult("Không tìm thấy thuộc tính để cập nhật.");
        }

        _mapper.Map(viewModel, attribute);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated Attribute: ID={Id}, Name={Name}", attribute.Id, attribute.Name);
            return OperationResult.SuccessResult($"Cập nhật thuộc tính '{attribute.Name}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật thuộc tính ID: {Id}", viewModel.Id);
            if (ex.InnerException?.Message?.Contains("UQ_Attribute_Slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult(message: "Slug này đã được sử dụng.", errors: new List<string> { "Slug này đã được sử dụng." });
            }
            return OperationResult.FailureResult(message: "Lỗi cơ sở dữ liệu khi cập nhật thuộc tính.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi cập nhật thuộc tính." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật thuộc tính ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi không mong muốn.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn." });
        }
    }

    public async Task<OperationResult> DeleteAttributeAsync(int id)
    {
        if (await HasRelatedValuesAsync(id))
        {
            var attribute = await _context.Set<domain.Entities.Attribute>().AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            string attributeName = attribute?.Name ?? "thuộc tính";
            _logger.LogWarning("Cannot delete Attribute {Name} (ID: {Id}) due to related values.", attributeName, id);
            return OperationResult.FailureResult($"Không thể xóa thuộc tính '{attributeName}' vì nó đang được sử dụng bởi các giá trị thuộc tính con.");
        }

        var attributeToDelete = new domain.Entities.Attribute { Id = id };
        _context.Entry(attributeToDelete).State = EntityState.Deleted;

        try
        {
            string attributeName = (await _context.Set<domain.Entities.Attribute>().AsNoTracking().FirstOrDefaultAsync(a => a.Id == id))?.Name ?? "thuộc tính"; // Re-fetch for name in message
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted Attribute: ID={Id}, Name={Name}", id, attributeName);
            return OperationResult.SuccessResult($"Xóa thuộc tính '{attributeName}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi quan hệ khi xóa thuộc tính ID {Id}", id);
            if (ex.InnerException?.Message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult("Không thể xóa vì thuộc tính đang được sử dụng.", errors: new List<string> { "Không thể xóa vì thuộc tính đang được sử dụng." });
            }
            return OperationResult.FailureResult("Lỗi cơ sở dữ liệu khi xóa thuộc tính.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi xóa thuộc tính." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi xóa thuộc tính ID {Id}", id);
            return OperationResult.FailureResult("Đã xảy ra lỗi không mong muốn khi xóa thuộc tính.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi xóa thuộc tính." });
        }
    }

    public async Task<bool> IsSlugUniqueAsync(string slug, int? ignoreId = null)
    {
        if (string.IsNullOrWhiteSpace(slug)) return false;

        var lowerSlug = slug.Trim().ToLower();
        var query = _context.Set<domain.Entities.Attribute>()
                            .Where(a => a.Slug.ToLower() == lowerSlug);

        if (ignoreId.HasValue && ignoreId.Value > 0)
        {
            query = query.Where(a => a.Id != ignoreId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<bool> HasRelatedValuesAsync(int attributeId)
    {
        return await _context.Set<AttributeValue>().AnyAsync(av => av.AttributeId == attributeId);
    }

    public async Task<List<SelectListItem>> GetAttributeSelectListAsync(int? selectedValue = null)
    {
        var attributes = await _context.Set<domain.Entities.Attribute>()
                    .OrderBy(a => a.Name)
                    .AsNoTracking()
                    .Select(a => new { a.Id, a.Name })
                    .ToListAsync();

        var items = new List<SelectListItem>
        {
             new SelectListItem { Value = "", Text = "-- Chọn thuộc tính --", Selected = !selectedValue.HasValue }
        };

        items.AddRange(attributes.Select(a => new SelectListItem
        {
            Value = a.Id.ToString(),
            Text = a.Name,
            Selected = selectedValue.HasValue && a.Id == selectedValue.Value
        }));

        return items;
    }
}