using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoRegister;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public class TagService : ITagService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<TagService> _logger;

    public TagService(ApplicationDbContext context, IMapper mapper, ILogger<TagService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IPagedList<TagListItemViewModel>> GetPagedTagsAsync(TagFilterViewModel filter, int pageNumber, int pageSize)
    {
        IQueryable<Tag> query = _context.Set<Tag>()
                                        .Include(t => t.ProductTags)
                                        .Include(t => t.ArticleTags);


        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(t => t.Name.ToLower().Contains(lowerSearchTerm) ||
                                t.Description != null && t.Description.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.Type.HasValue)
        {
            query = query.Where(t => t.Type == filter.Type.Value);
        }

        query = query.OrderBy(t => t.Name);

        IPagedList<TagListItemViewModel> pagedList = await query
            .ProjectTo<TagListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        return pagedList;
    }

    public async Task<TagViewModel?> GetTagByIdAsync(int id)
    {
        Tag? tag = await _context.Set<Tag>().AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

        return _mapper.Map<TagViewModel>(tag);
    }

    public async Task<OperationResult<int>> CreateTagAsync(TagViewModel viewModel)
    {
        if (await IsNameUniqueAsync(viewModel.Name, viewModel.Type))
        {
            return OperationResult<int>.FailureResult(message: $"Tên thẻ '{viewModel.Name}' đã tồn tại cho loại này.", errors: new List<string> { $"Tên thẻ '{viewModel.Name}' đã tồn tại cho loại này." });
        }

        var tag = _mapper.Map<Tag>(viewModel);
        _context.Add(tag);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created Tag: ID={Id}, Name={Name}, Type={Type}", tag.Id, tag.Name, tag.Type);
            return OperationResult<int>.SuccessResult(tag.Id, $"Thêm thẻ '{tag.Name}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi tạo thẻ mới '{Name}'", viewModel.Name);
            if (ex.InnerException?.Message?.Contains("UQ_Tag_Name_Type", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult<int>.FailureResult(message: $"Tên thẻ '{viewModel.Name}' đã tồn tại cho loại này.", errors: new List<string> { $"Tên thẻ '{viewModel.Name}' đã tồn tại cho loại này." });
            }
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi hệ thống khi tạo thẻ.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi tạo thẻ." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi tạo thẻ mới '{Name}'", viewModel.Name);
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi hệ thống khi tạo thẻ.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi tạo thẻ." });
        }
    }

    public async Task<OperationResult> UpdateTagAsync(TagViewModel viewModel)
    {
        if (await IsNameUniqueAsync(viewModel.Name, viewModel.Type, viewModel.Id))
        {
            return OperationResult.FailureResult(message: $"Tên thẻ '{viewModel.Name}' đã tồn tại cho loại này.", errors: new List<string> { $"Tên thẻ '{viewModel.Name}' đã tồn tại cho loại này." });
        }

        var tag = await _context.Set<Tag>().FirstOrDefaultAsync(t => t.Id == viewModel.Id);
        if (tag == null)
        {
            _logger.LogWarning("Tag not found for update. ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult("Không tìm thấy thẻ để cập nhật.");
        }

        _mapper.Map(viewModel, tag);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated Tag: ID={Id}, Name={Name}, Type={Type}", tag.Id, tag.Name, tag.Type);
            return OperationResult.SuccessResult($"Cập nhật thẻ '{tag.Name}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật thẻ '{Name}'", viewModel.Name);
            if (ex.InnerException?.Message?.Contains("UQ_Tag_Name_Type", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult(message: $"Tên thẻ '{viewModel.Name}' đã tồn tại cho loại này.", errors: new List<string> { $"Tên thẻ '{viewModel.Name}' đã tồn tại cho loại này." });
            }
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi hệ thống khi cập nhật thẻ.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi cập nhật thẻ." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật thẻ '{Name}'", viewModel.Name);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi hệ thống khi cập nhật thẻ.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi cập nhật thẻ." });
        }
    }

    public async Task<OperationResult> DeleteTagAsync(int id)
    {
        var checkResult = await CheckTagRelationsAsync(id);

        if (checkResult.name == null)
        {
            _logger.LogWarning("Tag not found for delete. ID: {Id}", id);
            return OperationResult.FailureResult("Không tìm thấy thẻ.");
        }

        int totalItems = checkResult.productCount + checkResult.articleCount;
        if (totalItems > 0)
        {
            string itemTypeName = checkResult.type.GetDisplayName().ToLowerInvariant();
            _logger.LogWarning("Cannot delete Tag {Name} (ID: {Id}) due to {TotalItems} related {ItemTypeName}.", checkResult.name, id, totalItems, itemTypeName);
            return OperationResult.FailureResult($"Không thể xóa thẻ '{checkResult.name}' vì đang được sử dụng bởi {totalItems} {itemTypeName}.");
        }

        var tagToDelete = new Tag { Id = id };
        _context.Entry(tagToDelete).State = EntityState.Deleted;


        try
        {
            string tagName = checkResult.name;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted Tag: ID={Id}, Name={Name}", id, tagName);
            return OperationResult.SuccessResult($"Xóa thẻ '{tagName}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi xóa thẻ ID {Id}", id);
            if (ex.InnerException?.Message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult("Không thể xóa thẻ vì đang được sử dụng.", errors: new List<string> { "Không thể xóa thẻ vì đang được sử dụng." });
            }
            return OperationResult.FailureResult("Lỗi cơ sở dữ liệu khi xóa thẻ.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi xóa thẻ." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi xóa thẻ ID {Id}", id);
            return OperationResult.FailureResult("Đã xảy ra lỗi không mong muốn khi xóa thẻ.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi xóa thẻ." });
        }
    }

    public async Task<bool> IsNameUniqueAsync(string name, TagType? type, int? ignoreId = null)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;

        var lowerName = name.Trim().ToLower();
        var query = _context.Set<Tag>()
                            .Where(t => t.Name.ToLower() == lowerName && t.Type == type);

        if (ignoreId.HasValue && ignoreId.Value > 0)
        {
            query = query.Where(t => t.Id != ignoreId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<(int productCount, int articleCount, TagType type, string name)> CheckTagRelationsAsync(int tagId)
    {
        var tagData = await _context.Set<Tag>()
          .Where(t => t.Id == tagId)
          .Select(t => new
          {
              Tag = t,
              ProductCount = t.ProductTags!.Count(),
              ArticleCount = t.ArticleTags!.Count(),
          })
          .AsNoTracking()
          .FirstOrDefaultAsync();

        if (tagData == null) return (0, 0, TagType.Product, null!);

        return (tagData.ProductCount, tagData.ArticleCount, tagData.Tag.Type, tagData.Tag.Name);
    }

    public async Task<List<SelectListItem>> GetTagSelectListAsync(TagType type, List<int>? selectedValues = null)
    {
        var tags = await _context.Set<Tag>()
                     .Where(t => t.Type == type)
                     .OrderBy(t => t.Name)
                     .AsNoTracking()
                     .Select(t => new { t.Id, t.Name })
                     .ToListAsync();

        var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Chọn thẻ --", Selected = selectedValues == null || !selectedValues.Any() }
        };

        items.AddRange(tags.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Name,
            Selected = selectedValues != null && selectedValues.Contains(t.Id)
        }));

        return items;
    }
}