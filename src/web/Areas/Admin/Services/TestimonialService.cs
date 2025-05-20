using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoRegister;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public class TestimonialService : ITestimonialService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<TestimonialService> _logger;

    public TestimonialService(ApplicationDbContext context, IMapper mapper, ILogger<TestimonialService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IPagedList<TestimonialListItemViewModel>> GetPagedTestimonialsAsync(TestimonialFilterViewModel filter, int pageNumber, int pageSize)
    {
        IQueryable<domain.Entities.Testimonial> query = _context.Set<domain.Entities.Testimonial>().AsNoTracking();


        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(t => t.ClientName.ToLower().Contains(lowerSearchTerm)
                              || t.ClientCompany != null && t.ClientCompany.ToLower().Contains(lowerSearchTerm)
                              || t.Content.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(t => t.IsActive == filter.IsActive.Value);
        }

        if (filter.Rating.HasValue && filter.Rating.Value > 0)
        {
            query = query.Where(t => t.Rating == filter.Rating.Value);
        }

        query = query.OrderBy(t => t.OrderIndex).ThenByDescending(t => t.UpdatedAt);

        IPagedList<TestimonialListItemViewModel> testimonialsPaged = await query
            .ProjectTo<TestimonialListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        return testimonialsPaged;
    }

    public async Task<TestimonialViewModel?> GetTestimonialByIdAsync(int id)
    {
        domain.Entities.Testimonial? testimonial = await _context.Set<domain.Entities.Testimonial>()
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync(t => t.Id == id);

        return _mapper.Map<TestimonialViewModel>(testimonial);
    }

    public async Task<OperationResult<int>> CreateTestimonialAsync(TestimonialViewModel viewModel)
    {
        var testimonial = _mapper.Map<domain.Entities.Testimonial>(viewModel);

        _context.Add(testimonial);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created Testimonial: ID={Id}, ClientName={ClientName}", testimonial.Id, testimonial.ClientName);
            return OperationResult<int>.SuccessResult(testimonial.Id, $"Thêm đánh giá của '{testimonial.ClientName}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi thêm đánh giá của {ClientName}", viewModel.ClientName);
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi hệ thống khi thêm đánh giá.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi thêm đánh giá." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi thêm đánh giá của {ClientName}", viewModel.ClientName);
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi hệ thống khi thêm đánh giá.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi thêm đánh giá." });
        }
    }

    public async Task<OperationResult> UpdateTestimonialAsync(TestimonialViewModel viewModel)
    {
        var testimonial = await _context.Set<domain.Entities.Testimonial>().FirstOrDefaultAsync(t => t.Id == viewModel.Id);
        if (testimonial == null)
        {
            _logger.LogWarning("Testimonial not found for update. ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult("Không tìm thấy đánh giá để cập nhật.");
        }

        _mapper.Map(viewModel, testimonial);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated Testimonial: ID={Id}, ClientName={ClientName}", testimonial.Id, testimonial.ClientName);
            return OperationResult.SuccessResult($"Cập nhật đánh giá của '{testimonial.ClientName}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật đánh giá của {ClientName} (ID: {Id})", viewModel.ClientName, viewModel.Id);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi hệ thống khi cập nhật đánh giá.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi cập nhật đánh giá." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật đánh giá của {ClientName} (ID: {Id})", viewModel.ClientName, viewModel.Id);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi hệ thống khi cập nhật đánh giá.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi cập nhật đánh giá." });
        }
    }

    public async Task<OperationResult> DeleteTestimonialAsync(int id)
    {
        domain.Entities.Testimonial? testimonial = await _context.Set<domain.Entities.Testimonial>().FindAsync(id);

        if (testimonial == null)
        {
            _logger.LogWarning("Testimonial not found for delete. ID: {Id}", id);
            return OperationResult.FailureResult("Không tìm thấy đánh giá.");
        }

        string clientName = testimonial.ClientName;

        _context.Remove(testimonial);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted Testimonial: ID={Id}, ClientName={ClientName}", id, clientName);
            return OperationResult.SuccessResult($"Xóa đánh giá của '{clientName}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi xóa đánh giá ID {Id}", id);
            if (ex.InnerException?.Message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult("Không thể xóa đánh giá vì đang được sử dụng.", errors: new List<string> { "Không thể xóa đánh giá vì đang được sử dụng." });
            }
            return OperationResult.FailureResult("Lỗi cơ sở dữ liệu khi xóa đánh giá.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi xóa đánh giá." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi xóa đánh giá ID {Id}", id);
            return OperationResult.FailureResult("Đã xảy ra lỗi không mong muốn khi xóa đánh giá.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi xóa đánh giá." });
        }
    }
}
