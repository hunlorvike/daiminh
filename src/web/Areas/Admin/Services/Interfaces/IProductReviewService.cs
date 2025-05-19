using shared.Enums;
using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface IProductReviewService
{
    Task<IPagedList<ProductReviewListItemViewModel>> GetPagedProductReviewsAsync(ProductReviewFilterViewModel filter, int pageNumber, int pageSize);

    Task<ProductReviewViewModel?> GetProductReviewByIdAsync(int id);

    Task<OperationResult> UpdateProductReviewStatusAsync(int reviewId, ReviewStatus newStatus);

    Task<OperationResult> DeleteProductReviewAsync(int id);

    Task RefillProductReviewViewModelFromDbAsync(ProductReviewViewModel viewModel);
}