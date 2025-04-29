using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Extensions;
using web.Areas.Admin.ViewModels.ProductReview;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize] // Add authorization if needed
public partial class ProductReviewController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductReviewController> _logger;

    public ProductReviewController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ProductReviewController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/ProductReview
    public async Task<IActionResult> Index(ProductReviewFilterViewModel filter, int page = 1, int pageSize = 25)
    {
        filter ??= new ProductReviewFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 25;

        IQueryable<ProductReview> query = _context.Set<ProductReview>()
                                             .Include(r => r.Product) // Include product to display name
                                             .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(r => (r.UserName != null && r.UserName.ToLower().Contains(lowerSearchTerm)) ||
                                     (r.UserEmail != null && r.UserEmail.ToLower().Contains(lowerSearchTerm)) ||
                                     r.Content.ToLower().Contains(lowerSearchTerm));
        }

        if (filter.ProductId.HasValue)
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

        var reviewsPaged = await query.ProjectTo<ProductReviewListItemViewModel>(_mapper.ConfigurationProvider)
                                       .ToPagedListAsync(pageNumber, currentPageSize);

        filter.ProductOptions = await LoadProductSelectListAsync(filter.ProductId);
        filter.StatusOptions = GetReviewStatusSelectList(filter.Status);
        filter.RatingOptions = GetRatingSelectList(filter.MinRating, filter.MaxRating);


        ProductReviewIndexViewModel viewModel = new()
        {
            Reviews = reviewsPaged,
            Filter = filter
        };

        return View(viewModel);
    }

    // GET: Admin/ProductReview/Edit/5 (View details and change status)
    public async Task<IActionResult> Edit(int id)
    {
        ProductReview? review = await _context.Set<ProductReview>()
                                         .Include(r => r.Product) // Include product to display name
                                         .AsNoTracking() // Use AsNoTracking for GET
                                         .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
        {
            return NotFound();
        }

        ProductReviewViewModel viewModel = _mapper.Map<ProductReviewViewModel>(review);
        PopulateViewModelSelectLists(viewModel);
        return View(viewModel);
    }

    // POST: Admin/ProductReview/Edit/5 (Only update status)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductReviewViewModel viewModel)
    {
        // Only validate the status property
        ModelState.Remove(nameof(viewModel.ProductId));
        ModelState.Remove(nameof(viewModel.ProductName));
        ModelState.Remove(nameof(viewModel.UserName));
        ModelState.Remove(nameof(viewModel.UserEmail));
        ModelState.Remove(nameof(viewModel.Rating));
        ModelState.Remove(nameof(viewModel.Content));
        ModelState.Remove(nameof(viewModel.CreatedAt));


        if (ModelState.IsValid)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            ProductReview? review = await _context.Set<ProductReview>().FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Only update the status property
            review.Status = viewModel.Status;

            try
            {
                _context.Update(review); // Mark as updated
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product review status with ID {ReviewId}.", id);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật trạng thái đánh giá.");
            }
        }

        // Reload data needed for the view model if validation fails
        ProductReview? reviewToDisplay = await _context.Set<ProductReview>()
                                         .Include(r => r.Product)
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync(r => r.Id == id);
        if (reviewToDisplay != null)
        {
            // Re-map to get product name etc.
            viewModel = _mapper.Map<ProductReviewViewModel>(reviewToDisplay);
        }
        PopulateViewModelSelectLists(viewModel);
        return View(viewModel);
    }


    // POST: Admin/ProductReview/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        ProductReview? review = await _context.Set<ProductReview>().FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
        {
            return Json(new { success = false, message = "Không tìm thấy đánh giá." });
        }

        try
        {
            string productName = review.Product?.Name ?? "Sản phẩm không xác định"; // Get product name before deleting review
            _context.Remove(review);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = $"Xóa đánh giá cho sản phẩm '{productName}' thành công." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product review with ID {ReviewId}.", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa đánh giá." });
        }
    }
}

public partial class ProductReviewController
{
    private void PopulateViewModelSelectLists(ProductReviewViewModel viewModel)
    {
        viewModel.StatusOptions = GetReviewStatusSelectList(viewModel.Status);
    }

    private async Task<List<SelectListItem>> LoadProductSelectListAsync(int? selectedValue = null)
    {
        var products = await _context.Set<Product>()
                          .OrderBy(p => p.Name)
                          .AsNoTracking()
                          .Select(p => new { p.Id, p.Name })
                          .ToListAsync();

        var items = new List<SelectListItem>
        {
             new SelectListItem { Value = "", Text = "-- Tất cả sản phẩm --", Selected = !selectedValue.HasValue }
        };

        items.AddRange(products.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = p.Name,
            Selected = selectedValue.HasValue && p.Id == selectedValue.Value
        }));

        return items;
    }

    private List<SelectListItem> GetReviewStatusSelectList(ReviewStatus? selectedStatus)
    {
        var items = Enum.GetValues(typeof(ReviewStatus))
            .Cast<ReviewStatus>()
            .Select(t => new SelectListItem
            {
                Value = ((int)t).ToString(),
                Text = t.GetDisplayName(),
                Selected = selectedStatus.HasValue && t == selectedStatus.Value
            })
             .OrderBy(t => t.Text) // Order alphabetically by display name
            .ToList();

        if (!selectedStatus.HasValue)
        {
            items.Insert(0, new SelectListItem { Value = "", Text = "Tất cả trạng thái", Selected = true });
        }
        else
        {
            items.Insert(0, new SelectListItem { Value = "", Text = "Tất cả trạng thái" });
        }


        return items;
    }

    private List<SelectListItem> GetRatingSelectList(int? minSelected, int? maxSelected)
    {
        var items = new List<SelectListItem>();
        for (int i = 1; i <= 5; i++)
        {
            items.Add(new SelectListItem { Value = i.ToString(), Text = $"{i} sao", Selected = (minSelected == i || maxSelected == i) });
        }

        // For filters, you typically select a single value for min/max, not both.
        // Let's provide options for Min Rating and Max Rating separately in the filter view,
        // so this helper might not be strictly needed for the filter dropdowns themselves,
        // but here's how you'd generate options 1-5.
        items.Insert(0, new SelectListItem { Value = "", Text = "Tất cả", Selected = !minSelected.HasValue && !maxSelected.HasValue }); // Add a default "All"

        return items; // This helper might need refinement based on actual filter UI implementation (two dropdowns or one range slider etc.)
    }
}