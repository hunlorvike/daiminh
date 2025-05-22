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
public class BannerService : IBannerService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<BannerService> _logger;

    public BannerService(ApplicationDbContext context, IMapper mapper, ILogger<BannerService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IPagedList<BannerListItemViewModel>> GetPagedBannersAsync(BannerFilterViewModel filter, int pageNumber, int pageSize)
    {
        IQueryable<Banner> query = _context.Set<Banner>()
                                       .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(b => b.Title.ToLower().Contains(lowerSearchTerm) ||
                                 b.Description != null && b.Description.ToLower().Contains(lowerSearchTerm) ||
                                 b.LinkUrl != null && b.LinkUrl.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.Type.HasValue)
        {
            query = query.Where(b => b.Type == filter.Type.Value);
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(b => b.IsActive == filter.IsActive.Value);
        }

        query = query.OrderBy(b => b.OrderIndex).ThenByDescending(b => b.CreatedAt);

        IPagedList<BannerListItemViewModel> bannersPaged = await query
            .ProjectTo<BannerListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        return bannersPaged;
    }

    public async Task<BannerViewModel?> GetBannerByIdAsync(int id)
    {
        Banner? banner = await _context.Set<Banner>()
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(b => b.Id == id);

        return _mapper.Map<BannerViewModel>(banner);
    }

    public async Task<OperationResult<int>> CreateBannerAsync(BannerViewModel viewModel)
    {
        var banner = _mapper.Map<Banner>(viewModel);

        _context.Add(banner);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created Banner: ID={Id}, Title={Title}, Type={Type}", banner.Id, banner.Title, banner.Type);
            return OperationResult<int>.SuccessResult(banner.Id, $"Thêm Banner '{banner.Title}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi tạo Banner: {Title}", viewModel.Title);
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi cơ sở dữ liệu khi lưu Banner.", errors: new List<string> { "Đã xảy ra lỗi cơ sở dữ liệu khi lưu Banner." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi tạo Banner: {Title}", viewModel.Title);
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi không mong muốn khi lưu Banner.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi lưu Banner." });
        }
    }

    public async Task<OperationResult> UpdateBannerAsync(BannerViewModel viewModel)
    {
        var banner = await _context.Set<Banner>().FirstOrDefaultAsync(b => b.Id == viewModel.Id);
        if (banner == null)
        {
            _logger.LogWarning("Banner not found for update. ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult("Không tìm thấy Banner để cập nhật.");
        }

        _mapper.Map(viewModel, banner);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated Banner: ID={Id}, Title={Title}, Type={Type}", banner.Id, banner.Title, banner.Type);
            return OperationResult.SuccessResult($"Cập nhật Banner '{banner.Title}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật Banner ID {Id}, Title {Title}", viewModel.Id, viewModel.Title);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật Banner.", errors: new List<string> { "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật Banner." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật Banner ID {Id}", viewModel.Id);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi không mong muốn khi cập nhật Banner.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi cập nhật Banner." });
        }
    }

    public async Task<OperationResult> DeleteBannerAsync(int id)
    {
        var banner = await _context.Set<Banner>().FirstOrDefaultAsync(b => b.Id == id);

        if (banner == null)
        {
            _logger.LogWarning("Banner not found for delete. ID: {Id}", id);
            return OperationResult.FailureResult("Không tìm thấy Banner.");
        }

        string bannerTitle = banner.Title;

        _context.Remove(banner);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted Banner: ID={Id}, Title={Title}", id, bannerTitle);
            return OperationResult.SuccessResult($"Xóa Banner '{bannerTitle}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi xóa Banner ID {Id}", id);
            if (ex.InnerException?.Message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult("Không thể xóa Banner vì đang được sử dụng.", errors: new List<string> { "Không thể xóa Banner vì đang được sử dụng." });
            }
            return OperationResult.FailureResult("Lỗi cơ sở dữ liệu khi xóa Banner.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi xóa Banner." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi xóa Banner ID {Id}", id);
            return OperationResult.FailureResult("Đã xảy ra lỗi không mong muốn khi xóa Banner.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi xóa Banner." });
        }
    }
}
