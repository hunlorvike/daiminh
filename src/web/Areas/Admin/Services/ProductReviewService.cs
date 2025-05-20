using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoRegister;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public class ProductReviewService : IProductReviewService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductReviewService> _logger;

    public ProductReviewService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ProductReviewService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IPagedList<ProductReviewListItemViewModel>> GetPagedProductReviewsAsync(ProductReviewFilterViewModel filter, int pageNumber, int pageSize)
    {
        IQueryable<ProductReview> query = _context.Set<ProductReview>()
                                         .Include(r => r.Product)
                                         .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(r => r.UserName != null && r.UserName.ToLower().Contains(lowerSearchTerm) ||
                                     r.UserEmail != null && r.UserEmail.ToLower().Contains(lowerSearchTerm) ||
                                     r.Content.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.ProductId.HasValue && filter.ProductId.Value > 0)
        {
            query = query.Where(r => r.ProductId == filter.ProductId.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(r => r.Status == filter.Status.Value);
        }

        if (filter.MinRating.HasValue)
        {
            query = query.Where(r => r.Rating >= filter.MinRating.Value);
        }

        if (filter.MaxRating.HasValue)
        {
            query = query.Where(r => r.Rating <= filter.MaxRating.Value);
        }

        query = query.OrderByDescending(r => r.CreatedAt);

        IPagedList<ProductReviewListItemViewModel> reviewsPaged = await query.ProjectTo<ProductReviewListItemViewModel>(_mapper.ConfigurationProvider)
                                   .ToPagedListAsync(pageNumber, pageSize);

        return reviewsPaged;
    }

    public async Task<ProductReviewViewModel?> GetProductReviewByIdAsync(int id)
    {
        ProductReview? review = await _context.Set<ProductReview>()
                                     .Include(r => r.Product)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null) return null;

        return _mapper.Map<ProductReviewViewModel>(review);
    }

    public async Task<OperationResult> UpdateProductReviewStatusAsync(int reviewId, ReviewStatus newStatus)
    {
        var review = await _context.Set<ProductReview>().Include(r => r.Product).FirstOrDefaultAsync(r => r.Id == reviewId);
        if (review == null)
        {
            _logger.LogWarning("ProductReview not found for status update. ID: {Id}", reviewId);
            return OperationResult.FailureResult("Không tìm thấy đánh giá.");
        }

        if (review.Status == newStatus)
        {
            _logger.LogInformation("ProductReview status {Status} for ID {Id} not changed.", review.Status, reviewId);
            return OperationResult.SuccessResult("Không có thay đổi trạng thái nào cần lưu.");
        }

        review.Status = newStatus;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated ProductReview status: ID={Id}, OldStatus={OldStatus}, NewStatus={NewStatus}", reviewId, _context.Entry(review).OriginalValues[nameof(ProductReview.Status)], newStatus);
            string productName = review.Product?.Name ?? "[ẩn]";
            return OperationResult.SuccessResult($"Cập nhật trạng thái đánh giá cho sản phẩm '{productName}' thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật trạng thái đánh giá ID: {Id}", reviewId);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật trạng thái đánh giá.", errors: new List<string> { "Đã xảy ra lỗi cơ sở dữ liệu khi cập nhật trạng thái đánh giá." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi không xác định khi cập nhật trạng thái đánh giá ID: {Id}", reviewId);
            return OperationResult.FailureResult(message: "Đã xảy ra lỗi hệ thống khi cập nhật trạng thái đánh giá.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi cập nhật trạng thái đánh giá." });
        }
    }


    public async Task<OperationResult> DeleteProductReviewAsync(int id)
    {
        var review = await _context.Set<ProductReview>()
                               .Include(r => r.Product)
                               .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
        {
            _logger.LogWarning("ProductReview not found for delete. ID: {Id}", id);
            return OperationResult.FailureResult("Không tìm thấy đánh giá.");
        }

        string productName = review.Product?.Name ?? "[Không rõ]"; // Get product name before deleting
        string reviewContentSnippet = review.Content?.Length > 50 ? review.Content.Substring(0, 50) + "..." : review.Content ?? "";

        _context.Remove(review);

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted ProductReview: ID={Id}, Product={Product}, Content='{ContentSnippet}'", id, productName, reviewContentSnippet);
            return OperationResult.SuccessResult($"Xóa đánh giá cho sản phẩm '{productName}' thành công.");
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
            return OperationResult.FailureResult("Đã xảy ra lỗi hệ thống khi xóa đánh giá.", errors: new List<string> { "Đã xảy ra lỗi hệ thống khi xóa đánh giá." });
        }
    }

    public async Task RefillProductReviewViewModelFromDbAsync(ProductReviewViewModel viewModel)
    {
        var reviewFromDb = await _context.Set<ProductReview>()
            .Include(r => r.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == viewModel.Id);

        if (reviewFromDb != null)
        {
            viewModel.ProductName = reviewFromDb.Product!.Name;
            viewModel.ProductId = reviewFromDb.ProductId;
            viewModel.UserEmail = reviewFromDb.UserEmail;
            viewModel.UserName = reviewFromDb.UserName;
            viewModel.Rating = reviewFromDb.Rating;
            viewModel.Content = reviewFromDb.Content;
            viewModel.CreatedAt = reviewFromDb.CreatedAt;
        }
    }
}