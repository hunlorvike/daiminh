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
public class FAQService : IFAQService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<FAQService> _logger;
    private readonly ICategoryService _categoryService;

    public FAQService(ApplicationDbContext context, IMapper mapper, ILogger<FAQService> logger, ICategoryService categoryService)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _categoryService = categoryService;
    }

    public async Task<IPagedList<FAQListItemViewModel>> GetPagedFAQsAsync(FAQFilterViewModel filter, int pageNumber, int pageSize)
    {
        IQueryable<domain.Entities.FAQ> query = _context.Set<domain.Entities.FAQ>()
                                    .Include(f => f.Category)
                                    .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(f => f.Question.ToLower().Contains(lowerSearchTerm)
                                  || f.Answer.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.CategoryId.HasValue && filter.CategoryId.Value > 0)
        {
            query = query.Where(f => f.CategoryId == filter.CategoryId.Value);
        }

        query = query
            .OrderBy(f => f.OrderIndex)
            .ThenBy(f => f.Question);

        IPagedList<FAQListItemViewModel> faqsPaged = await query
            .ProjectTo<FAQListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        return faqsPaged;
    }

    public async Task<FAQViewModel?> GetFAQByIdAsync(int id)
    {
        domain.Entities.FAQ? faq = await _context.Set<domain.Entities.FAQ>()
                                 .Include(f => f.Category)
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(f => f.Id == id);

        if (faq == null) return null;

        return _mapper.Map<FAQViewModel>(faq);
    }

    public async Task<OperationResult<int>> CreateFAQAsync(FAQViewModel viewModel)
    {
        var categoryExists = await _categoryService.GetCategoryByIdAsync(viewModel.CategoryId);
        if (categoryExists == null)
        {
            return OperationResult<int>.FailureResult(message: "Danh mục cha không tồn tại.", errors: new List<string> { "Danh mục cha không tồn tại." });
        }

        var faq = _mapper.Map<domain.Entities.FAQ>(viewModel);
        _context.Add(faq);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created FAQ: ID={Id}, Question={Question}, CategoryId={CategoryId}", faq.Id, faq.Question, faq.CategoryId);
            return OperationResult<int>.SuccessResult(faq.Id, $"Thêm FAQ '{faq.Question}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi tạo FAQ: {Question}", viewModel.Question);
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi cơ sở dữ liệu khi thêm FAQ.", errors: new List<string> { "Đã xảy ra lỗi cơ sở dữ liệu khi thêm FAQ." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi tạo FAQ.");
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi hệ thống khi thêm FAQ.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi thêm FAQ." });
        }
    }

    public async Task<OperationResult> UpdateFAQAsync(FAQViewModel viewModel)
    {
        var categoryExists = await _categoryService.GetCategoryByIdAsync(viewModel.CategoryId);
        if (categoryExists == null)
        {
            return OperationResult.FailureResult(message: "Danh mục cha không tồn tại.", errors: new List<string> { "Danh mục cha không tồn tại." });
        }

        var faq = await _context.Set<domain.Entities.FAQ>().FirstOrDefaultAsync(f => f.Id == viewModel.Id);
        if (faq == null)
        {
            _logger.LogWarning("FAQ not found for update. ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult("Không tìm thấy FAQ để cập nhật.");
        }

        _mapper.Map(viewModel, faq);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated FAQ: ID={Id}, Question={Question}, CategoryId={CategoryId}", faq.Id, faq.Question, faq.CategoryId);
            return OperationResult.SuccessResult($"Cập nhật FAQ '{faq.Question}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật FAQ ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật FAQ.", errors: new List<string> { "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật FAQ." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật FAQ ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi hệ thống khi cập nhật FAQ.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi cập nhật FAQ." });
        }
    }

    public async Task<OperationResult> DeleteFAQAsync(int id)
    {
        var faq = await _context.Set<domain.Entities.FAQ>().FindAsync(id);
        if (faq == null)
        {
            _logger.LogWarning("FAQ not found for delete. ID: {Id}", id);
            return OperationResult.FailureResult("Không tìm thấy FAQ.");
        }

        string question = faq.Question;

        _context.Remove(faq);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted FAQ: ID={Id}, Question={Question}", id, question);
            return OperationResult.SuccessResult($"Xóa FAQ '{question}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi xóa FAQ ID {Id}", id);
            if (ex.InnerException?.Message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult("Không thể xóa FAQ vì đang được sử dụng.", errors: new List<string> { "Không thể xóa FAQ vì đang được sử dụng." });
            }
            return OperationResult.FailureResult("Lỗi cơ sở dữ liệu khi xóa FAQ.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi xóa FAQ." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi xóa FAQ ID {Id}", id);
            return OperationResult.FailureResult("Đã xảy ra lỗi không mong muốn khi xóa FAQ.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi xóa FAQ." });
        }
    }
}