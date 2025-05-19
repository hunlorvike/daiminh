using AutoMapper;
using AutoMapper.QueryableExtensions;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Services;

public class NewsletterService : INewsletterService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<NewsletterService> _logger;

    public NewsletterService(ApplicationDbContext context, IMapper mapper, ILogger<NewsletterService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IPagedList<NewsletterListItemViewModel>> GetPagedNewslettersAsync(NewsletterFilterViewModel filter, int pageNumber, int pageSize)
    {
        IQueryable<domain.Entities.Newsletter> query = _context.Set<domain.Entities.Newsletter>().AsNoTracking(); // Readonly for list

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(n => n.Email.ToLower().Contains(lowerSearchTerm) ||
                                     n.Name != null && n.Name.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(n => n.IsActive == filter.IsActive.Value);
        }

        // Apply ordering before projection and pagination
        query = query.OrderByDescending(n => n.CreatedAt);

        IPagedList<NewsletterListItemViewModel> newslettersPaged = await query
            .ProjectTo<NewsletterListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        return newslettersPaged;
    }

    public async Task<NewsletterViewModel?> GetNewsletterByIdAsync(int id)
    {
        domain.Entities.Newsletter? newsletter = await _context.Set<domain.Entities.Newsletter>()
                                       .AsNoTracking() // Readonly operation
                                       .FirstOrDefaultAsync(n => n.Id == id);

        return _mapper.Map<NewsletterViewModel>(newsletter);
    }

    public async Task<OperationResult<int>> CreateNewsletterAsync(NewsletterViewModel viewModel, string? ipAddress, string? userAgent)
    {
        // **Service handles DB-related validation**
        if (await IsEmailUniqueAsync(viewModel.Email!)) // Email is required by validation
        {
            return OperationResult<int>.FailureResult(message: "Email này đã tồn tại.", errors: new List<string> { "Email này đã tồn tại." });
        }

        var newsletter = _mapper.Map<domain.Entities.Newsletter>(viewModel);

        // **Service handles business logic: Set tracking info**
        newsletter.IpAddress = ipAddress;
        newsletter.UserAgent = userAgent;

        // **Service handles business logic: Apply status tracking timestamps**
        ApplyStatusTracking(newsletter);

        _context.Add(newsletter);
        // CreatedAt is set automatically by BaseEntity

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created Newsletter: ID={Id}, Email={Email}", newsletter.Id, newsletter.Email);
            return OperationResult<int>.SuccessResult(newsletter.Id, $"Thêm đăng ký email '{newsletter.Email}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi tạo đăng ký mới cho email: {Email}", viewModel.Email);
            // Check for specific constraint violations if possible/needed
            if (ex.InnerException?.Message?.Contains("UQ_Newsletter_Email", StringComparison.OrdinalIgnoreCase) == true) // Example unique constraint name
            {
                return OperationResult<int>.FailureResult(message: "Email này đã tồn tại.", errors: new List<string> { "Email này đã tồn tại." });
            }
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi cơ sở dữ liệu khi lưu đăng ký.", errors: new List<string> { "Đã xảy ra lỗi cơ sở dữ liệu khi lưu đăng ký." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi tạo đăng ký mới cho email: {Email}", viewModel.Email);
            return OperationResult<int>.FailureResult(message: "Đã xảy ra lỗi hệ thống khi lưu đăng ký.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi lưu đăng ký." });
        }
    }

    public async Task<OperationResult> UpdateNewsletterAsync(NewsletterViewModel viewModel)
    {
        // **Service handles DB-related validation**
        if (await IsEmailUniqueAsync(viewModel.Email!, viewModel.Id)) // Email is required by validation
        {
            return OperationResult.FailureResult(message: "Email này đã tồn tại.", errors: new List<string> { "Email này đã tồn tại." });
        }

        var newsletter = await _context.Set<domain.Entities.Newsletter>().FirstOrDefaultAsync(n => n.Id == viewModel.Id);
        if (newsletter == null)
        {
            _logger.LogWarning("Newsletter not found for update. ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult("Không tìm thấy đăng ký để cập nhật.");
        }

        // Keep original tracking info if they are not editable via ViewModel
        string? originalIp = newsletter.IpAddress;
        string? originalUserAgent = newsletter.UserAgent;
        DateTime originalCreatedAt = newsletter.CreatedAt;


        _mapper.Map(viewModel, newsletter); // Map updated values

        // Restore tracking info - ViewModel might not have them
        newsletter.IpAddress = originalIp;
        newsletter.UserAgent = originalUserAgent;
        newsletter.CreatedAt = originalCreatedAt;

        // **Service handles business logic: Apply status tracking timestamps**
        ApplyStatusTracking(newsletter);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated Newsletter: ID={Id}, Email={Email}, IsActive={IsActive}", newsletter.Id, newsletter.Email, newsletter.IsActive);
            return OperationResult.SuccessResult($"Cập nhật đăng ký email '{newsletter.Email}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật đăng ký email: {Email} (ID: {Id})", viewModel.Email, viewModel.Id);
            // Check for specific constraint violations if possible/needed
            if (ex.InnerException?.Message?.Contains("UQ_Newsletter_Email", StringComparison.OrdinalIgnoreCase) == true) // Example unique constraint name
            {
                return OperationResult.FailureResult(message: "Email này đã tồn tại.", errors: new List<string> { "Email này đã tồn tại." });
            }
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi hệ thống khi cập nhật đăng ký.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi cập nhật đăng ký." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật đăng ký email: {Email} (ID: {Id})", viewModel.Email, viewModel.Id);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi hệ thống khi cập nhật đăng ký.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi cập nhật đăng ký." });
        }
    }

    public async Task<OperationResult> DeleteNewsletterAsync(int id)
    {
        var newsletter = await _context.Set<domain.Entities.Newsletter>().FindAsync(id);

        if (newsletter == null)
        {
            _logger.LogWarning("Newsletter not found for delete. ID: {Id}", id);
            return OperationResult.FailureResult("Không tìm thấy đăng ký.");
        }

        string email = newsletter.Email; // Get email before deleting

        // No complex business logic check like related items for Newsletter

        _context.Remove(newsletter);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted Newsletter: ID={Id}, Email={Email}", id, email);
            return OperationResult.SuccessResult($"Xóa đăng ký email '{email}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi xóa đăng ký email ID {Id}", id);
            // Check for specific constraint violations if possible/needed (unlikely for Newsletter unless it has FKs)
            if (ex.InnerException?.Message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) == true)
            {
                return OperationResult.FailureResult("Không thể xóa đăng ký vì đang được sử dụng.", errors: new List<string> { "Không thể xóa đăng ký vì đang được sử dụng." });
            }
            return OperationResult.FailureResult("Lỗi cơ sở dữ liệu khi xóa đăng ký.", errors: new List<string> { "Lỗi cơ sở dữ liệu khi xóa đăng ký." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi xóa đăng ký email ID {Id}", id);
            return OperationResult.FailureResult("Đã xảy ra lỗi không mong muốn khi xóa đăng ký.", errors: new List<string> { "Đã xảy ra lỗi không mong muốn khi xóa đăng ký." });
        }
    }

    // **Service handles DB-related validation logic**
    public async Task<bool> IsEmailUniqueAsync(string email, int? ignoreId = null)
    {
        if (string.IsNullOrWhiteSpace(email)) return false; // Email cannot be empty for uniqueness check

        var lowerEmail = email.Trim().ToLower();
        var query = _context.Set<domain.Entities.Newsletter>()
                            .Where(n => n.Email.ToLower() == lowerEmail);

        if (ignoreId.HasValue && ignoreId.Value > 0)
        {
            query = query.Where(n => n.Id != ignoreId.Value);
        }

        return await query.AnyAsync();
    }

    // **Service handles business logic: Apply status tracking timestamps**
    private void ApplyStatusTracking(domain.Entities.Newsletter newsletter)
    {
        if (newsletter.IsActive)
        {
            newsletter.UnsubscribedAt = null;
            // Only set ConfirmedAt if it hasn't been set before (i.e., it's a new confirmation)
            // Or always set if IsActive means "currently confirmed"
            // Original code just set ConfirmedAt = DateTime.UtcNow if IsActive. Let's follow that.
            newsletter.ConfirmedAt = DateTime.UtcNow;
        }
        else
        {
            newsletter.UnsubscribedAt = DateTime.UtcNow;
            // Only set ConfirmedAt to null if it's becoming inactive
            // Original code set ConfirmedAt = null if not IsActive. Let's follow that.
            newsletter.ConfirmedAt = null;
        }
    }

    // Removed SetTrackingInfo as IP/UA are passed as parameters
    // private void SetTrackingInfo(Newsletter newsletter) { ... }
}
