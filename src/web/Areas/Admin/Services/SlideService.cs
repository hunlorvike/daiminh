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
public class SlideService : ISlideService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<SlideService> _logger;

    public SlideService(ApplicationDbContext context, IMapper mapper, ILogger<SlideService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IPagedList<SlideListItemViewModel>> GetPagedSlidesAsync(SlideFilterViewModel filter, int pageNumber, int pageSize)
    {
        IQueryable<Slide> query = _context.Set<Slide>()
                                      .AsNoTracking();


        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(s => s.Title.ToLower().Contains(lowerSearchTerm) ||
                                 s.Subtitle != null && s.Subtitle.ToLower().Contains(lowerSearchTerm) ||
                                 s.Description != null && s.Description.ToLower().Contains(lowerSearchTerm) ||
                                 s.CtaText != null && s.CtaText.ToLower().Contains(lowerSearchTerm) ||
                                 s.CtaLink != null && s.CtaLink.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(s => s.IsActive == filter.IsActive.Value);
        }

        // Apply ordering before projection and pagination
        query = query.OrderBy(s => s.OrderIndex).ThenByDescending(s => s.CreatedAt);

        IPagedList<SlideListItemViewModel> slidesPaged = await query
            .ProjectTo<SlideListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        return slidesPaged;
    }

    public async Task<SlideViewModel?> GetSlideByIdAsync(int id)
    {
        Slide? slide = await _context.Set<Slide>()
                                 .AsNoTracking() // Readonly operation
                                 .FirstOrDefaultAsync(s => s.Id == id);

        return _mapper.Map<SlideViewModel>(slide);
    }

    public async Task<OperationResult<int>> CreateSlideAsync(SlideViewModel viewModel)
    {
        // No DB-specific validation logic needed for Slide (no unique slug/name typically)

        var slide = _mapper.Map<Slide>(viewModel);
        // CreatedAt is set automatically by BaseEntity

        _context.Add(slide);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created Slide: ID={Id}, Title={Title}", slide.Id, slide.Title);
            return OperationResult<int>.SuccessResult(slide.Id, $"Thêm Slide '{slide.Title}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi tạo Slide: {Title}", viewModel.Title);
            // Generic DB error for now, unless there are specific constraints
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi cơ sở dữ liệu khi lưu Slide.", errors: new List<string> { "Đã xảy ra lỗi cơ sở dữ liệu khi lưu Slide." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi tạo Slide: {Title}", viewModel.Title);
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi không mong muốn khi lưu Slide.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi lưu Slide." });
        }
    }

    public async Task<OperationResult> UpdateSlideAsync(SlideViewModel viewModel)
    {
        // No DB-specific validation logic needed for Slide update

        var slide = await _context.Set<Slide>().FirstOrDefaultAsync(s => s.Id == viewModel.Id);
        if (slide == null)
        {
            _logger.LogWarning("Slide not found for update. ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult("Không tìm thấy Slide để cập nhật.");
        }

        _mapper.Map(viewModel, slide); // Map updated values
                                       // UpdatedAt is set automatically by BaseEntity

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated Slide: ID={Id}, Title={Title}", slide.Id, slide.Title);
            return OperationResult.SuccessResult($"Cập nhật Slide '{slide.Title}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật Slide ID {Id}, Title {Title}", viewModel.Id, viewModel.Title);
            // Generic DB error for now
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật Slide.", errors: new List<string> { "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật Slide." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật Slide ID {Id}", viewModel.Id);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi không mong muốn khi cập nhật Slide.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi cập nhật Slide." });
        }
    }

    public async Task<OperationResult> DeleteSlideAsync(int id)
    {
        var slide = await _context.Set<Slide>().FirstOrDefaultAsync(s => s.Id == id);

        if (slide == null)
        {
            _logger.LogWarning("Slide not found for delete. ID: {Id}", id);
            return OperationResult.FailureResult("Không tìm thấy Slide.");
        }

        string slideTitle = slide.Title; // Get title before deleting

        // No complex business logic check like related items for Slide

        _context.Remove(slide);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted Slide: ID={Id}, Title={Title}", id, slideTitle);
            return OperationResult.SuccessResult($"Xóa Slide '{slideTitle}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi xóa Slide ID {Id}", id);
            // Check for specific constraint violations if possible/needed (unlikely for Slide unless it has FKs)
            if (ex.InnerException?.Message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult("Không thể xóa Slide vì đang được sử dụng.", errors: new List<string> { "Không thể xóa Slide vì đang được sử dụng." });
            }
            return OperationResult.FailureResult("Lỗi cơ sở dữ liệu khi xóa Slide.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi xóa Slide." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi xóa Slide ID {Id}", id);
            return OperationResult.FailureResult("Đã xảy ra lỗi không mong muốn khi xóa Slide.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi xóa Slide." });
        }
    }
}
