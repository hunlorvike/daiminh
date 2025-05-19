using AutoMapper;
using AutoMapper.QueryableExtensions;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Services;

public class PageService : IPageService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<PageService> _logger;

    public PageService(ApplicationDbContext context, IMapper mapper, ILogger<PageService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IPagedList<PageListItemViewModel>> GetPagedPagesAsync(PageFilterViewModel filter, int pageNumber, int pageSize)
    {
        IQueryable<domain.Entities.Page> query = _context.Set<domain.Entities.Page>()
                                    .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(p => p.Title.ToLower().Contains(lowerSearchTerm) ||
                                 p.Slug.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(p => p.Status == filter.Status.Value);
        }

        query = query.OrderByDescending(p => p.UpdatedAt).ThenBy(p => p.Title);

        IPagedList<PageListItemViewModel> pagesPaged = await query
            .ProjectTo<PageListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        return pagesPaged;
    }

    public async Task<PageViewModel?> GetPageByIdAsync(int id)
    {
        domain.Entities.Page? page = await _context.Set<domain.Entities.Page>()
                               .AsNoTracking()
                               .FirstOrDefaultAsync(p => p.Id == id);

        if (page == null) return null;

        return _mapper.Map<PageViewModel>(page);
    }

    public async Task<OperationResult<int>> CreatePageAsync(PageViewModel viewModel)
    {
        if (await IsSlugUniqueAsync(viewModel.Slug!))
        {
            return OperationResult<int>.FailureResult(message: "Slug này đã được sử dụng.", errors: new List<string> { "Slug này đã được sử dụng." });
        }

        var page = _mapper.Map<domain.Entities.Page>(viewModel);

        if (page.Status == PublishStatus.Published && page.PublishedAt == null)
        {
            page.PublishedAt = DateTime.UtcNow;
        }

        _context.Add(page);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created Page: ID={Id}, Title={Title}, Slug={Slug}", page.Id, page.Title, page.Slug);
            return OperationResult<int>.SuccessResult(page.Id, $"Thêm trang '{page.Title}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi tạo trang: {Title}", viewModel.Title);
            if (ex.InnerException?.Message?.Contains("UQ_Page_Slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult<int>.FailureResult(message: "Slug này đã được sử dụng.", errors: new List<string> { "Slug này đã được sử dụng." });
            }
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi cơ sở dữ liệu khi lưu trang.", errors: new List<string> { "Đã xảy ra lỗi cơ sở dữ liệu khi lưu trang." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi tạo trang: {Title}", viewModel.Title);
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi không mong muốn khi lưu trang.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi lưu trang." });
        }
    }

    public async Task<OperationResult> UpdatePageAsync(PageViewModel viewModel)
    {
        if (await IsSlugUniqueAsync(viewModel.Slug!, viewModel.Id))
        {
            return OperationResult.FailureResult(message: "Slug này đã được sử dụng.", errors: new List<string> { "Slug này đã được sử dụng." });
        }

        var page = await _context.Set<domain.Entities.Page>().FirstOrDefaultAsync(p => p.Id == viewModel.Id);
        if (page == null)
        {
            _logger.LogWarning("Page not found for update. ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult("Không tìm thấy trang để cập nhật.");
        }

        var oldStatus = page.Status;

        _mapper.Map(viewModel, page);

        if (oldStatus != PublishStatus.Published && page.Status == PublishStatus.Published && page.PublishedAt == null)
        {
            page.PublishedAt = DateTime.UtcNow;
        }

        try
        {
            _context.Update(page);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated Page: ID={Id}, Title={Title}, Slug={Slug}", page.Id, page.Title, page.Slug);
            return OperationResult.SuccessResult($"Cập nhật trang '{page.Title}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật trang ID {Id}, Title {Title}", viewModel.Id, viewModel.Title);
            if (ex.InnerException?.Message?.Contains("UQ_Page_Slug", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult(message: "Slug này đã được sử dụng.", errors: new List<string> { "Slug này đã được sử dụng." });
            }
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật trang.", errors: new List<string> { "Đã xảy ra lỗi cơ sở dữ động khi cập nhật trang." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật trang ID {Id}", viewModel.Id);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi không mong muốn khi cập nhật trang.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi cập nhật trang." });
        }
    }


    public async Task<OperationResult> DeletePageAsync(int id)
    {
        var page = await _context.Set<domain.Entities.Page>().FirstOrDefaultAsync(p => p.Id == id);

        if (page == null)
        {
            _logger.LogWarning("Page not found for delete. ID: {Id}", id);
            return OperationResult.FailureResult("Không tìm thấy trang.");
        }

        string pageTitle = page.Title;

        _context.Remove(page);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted Page: ID={Id}, Title={Title}", id, pageTitle);
            return OperationResult.SuccessResult($"Xóa trang '{pageTitle}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi xóa trang ID {Id}", id);
            if (ex.InnerException?.Message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult("Không thể xóa trang vì đang được sử dụng.", errors: new List<string> { "Không thể xóa trang vì đang được sử dụng." });
            }
            return OperationResult.FailureResult("Lỗi cơ sở dữ liệu khi xóa trang.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi xóa trang." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi xóa trang ID {Id}", id);
            return OperationResult.FailureResult($"Không thể xóa trang '{pageTitle}'.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi xóa trang." });
        }
    }

    public async Task<bool> IsSlugUniqueAsync(string slug, int? ignoreId = null)
    {
        if (string.IsNullOrWhiteSpace(slug)) return false;

        var lowerSlug = slug.Trim().ToLower();
        var query = _context.Set<domain.Entities.Page>()
                            .Where(p => p.Slug.ToLower() == lowerSlug);

        if (ignoreId.HasValue && ignoreId.Value > 0)
        {
            query = query.Where(p => p.Id != ignoreId.Value);
        }

        return await query.AnyAsync();
    }
}