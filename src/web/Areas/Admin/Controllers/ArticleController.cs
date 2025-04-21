// web.Areas.Admin.Controllers/ArticleController.cs
using AutoMapper;
using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Extensions;
using web.Areas.Admin.ViewModels.Article;
using web.Areas.Admin.ViewModels.Shared; // For SeoViewModel
using X.PagedList;
using X.PagedList.Extensions;
using System.Security.Claims;
using AutoMapper.QueryableExtensions;
using X.PagedList.EF; // For AuthorId

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize] // Apply authorization as needed
public class ArticleController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ArticleController> _logger;
    private readonly IValidator<ArticleViewModel> _validator; // Inject validator for manual validation if needed, though MVC integration usually handles this.

    public ArticleController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ArticleController> logger,
        IValidator<ArticleViewModel> validator) // Inject validator
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    // GET: Admin/Article
    public async Task<IActionResult> Index(ArticleFilterViewModel filter, int page = 1, int pageSize = 25)
    {
        filter ??= new ArticleFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 25;

        IQueryable<Article> query = _context.Set<Article>()
                                             .Include(a => a.Category) // Include Category for CategoryName
                                             .Include(a => a.ArticleTags) // Include ArticleTags for count
                                             .Include(a => a.ArticleProducts) // Include ArticleProducts for count
                                             .AsNoTracking(); // Use AsNoTracking for read-only operations

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(a => a.Title.ToLower().Contains(lowerSearchTerm) ||
                                     (a.Summary != null && a.Summary.ToLower().Contains(lowerSearchTerm)) ||
                                     (a.Content != null && a.Content.ToLower().Contains(lowerSearchTerm)));
        }

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(a => a.CategoryId == filter.CategoryId.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(a => a.Status == filter.Status.Value);
        }

        if (filter.IsFeatured.HasValue)
        {
            query = query.Where(a => a.IsFeatured == filter.IsFeatured.Value);
        }

        query = query.OrderByDescending(a => a.PublishedAt) // Order by published date, then creation date
                     .ThenByDescending(a => a.CreatedAt);

        // Note: Including ArticleTags and ArticleProducts just for counting might be inefficient
        // on large datasets. An alternative is to use Select and COUNT inside the query,
        // but mapping becomes more complex. For this pattern, including and letting AutoMapper count is simpler.

        var articlesPaged = await query.ProjectTo<ArticleListItemViewModel>(_mapper.ConfigurationProvider) // Use ProjectTo for efficiency with AutoMapper
                                       .ToPagedListAsync(pageNumber, currentPageSize);

        filter.CategoryOptions = await LoadCategorySelectListAsync(CategoryType.Article, filter.CategoryId);
        filter.StatusOptions = GetPublishStatusSelectList(filter.Status);
        filter.IsFeaturedOptions = GetYesNoSelectList(filter.IsFeatured, "Tất cả");


        ArticleIndexViewModel viewModel = new()
        {
            Articles = articlesPaged,
            Filter = filter
        };

        return View(viewModel);
    }


    // GET: Admin/Article/Create
    public async Task<IActionResult> Create()
    {
        ArticleViewModel viewModel = new()
        {
            IsFeatured = false,
            Status = PublishStatus.Draft,
            PublishedAt = DateTime.UtcNow, // Default to now, can be changed
            Seo = new ViewModels.Shared.SeoViewModel // Initialize SeoViewModel
            {
                SitemapPriority = 0.5,
                SitemapChangeFrequency = "monthly"
            }
        };

        await PopulateViewModelSelectListsAsync(viewModel);

        return View(viewModel);
    }

    // POST: Admin/Article/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ArticleViewModel viewModel)
    {
        // MVC's validation pipeline automatically calls the registered FluentValidator
        if (ModelState.IsValid)
        {
            Article article = _mapper.Map<Article>(viewModel);

            // Set AuthorId (Assuming user ID is stored in a claim)
            article.AuthorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // Optionally set AuthorName/Avatar from user profile if available and not set in ViewModel
            if (string.IsNullOrWhiteSpace(article.AuthorName))
            {
                article.AuthorName = User.FindFirst(ClaimTypes.Name)?.Value ?? "Admin"; // Fallback
            }

            // Handle many-to-many relationships (Tags, Products)
            await UpdateArticleRelationshipsAsync(article, viewModel.SelectedTagIds, viewModel.SelectedProductIds);

            _context.Add(article);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Article created successfully: {Title} (ID: {Id})", article.Title, article.Id);
                TempData["SuccessMessage"] = $"Đã thêm bài viết '{article.Title}' thành công.";
                return RedirectToAction(nameof(Index)); // Redirect to index after creation
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error creating article: {Title}", viewModel.Title);
                // Check for unique slug violation
                if (ex.InnerException?.Message.Contains("idx_articles_slug", StringComparison.OrdinalIgnoreCase) == true ||
                    ex.InnerException?.Message.Contains("articles.slug", StringComparison.OrdinalIgnoreCase) == true)
                {
                    ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã tồn tại, vui lòng chọn slug khác.");
                }
                else if (ex.InnerException?.Message.Contains("FK_articles_categories", StringComparison.OrdinalIgnoreCase) == true)
                {
                    ModelState.AddModelError(nameof(viewModel.CategoryId), "Danh mục được chọn không tồn tại.");
                }
                else
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu bài viết.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating article: {Title}", viewModel.Title);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi lưu bài viết.");
            }

            // If we are returning to the view, re-populate SelectLists
            await PopulateViewModelSelectListsAsync(viewModel);
        }

        return View(viewModel);
    }


    // GET: Admin/Article/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        Article? article = await _context.Set<Article>()
                                         .Include(a => a.ArticleTags) // Include for mapping SelectedTagIds
                                         .Include(a => a.ArticleProducts) // Include for mapping SelectedProductIds
                                         .AsNoTracking() // Use AsNoTracking for GET requests
                                         .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null)
        {
            _logger.LogWarning("Article not found for editing: ID {Id}", id);
            return NotFound();
        }

        ArticleViewModel viewModel = _mapper.Map<ArticleViewModel>(article);

        await PopulateViewModelSelectListsAsync(viewModel);

        return View(viewModel);
    }

    // POST: Admin/Article/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ArticleViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            _logger.LogWarning("Bad request: ID mismatch during Article edit. Path ID: {PathId}, ViewModel ID: {ViewModelId}", id, viewModel.Id);
            return BadRequest();
        }

        // MVC's validation pipeline automatically calls the registered FluentValidator
        if (ModelState.IsValid)
        {
            // Fetch the entity including relationships to update
            Article? article = await _context.Set<Article>()
                                             .Include(a => a.ArticleTags)
                                             .Include(a => a.ArticleProducts)
                                             .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                _logger.LogWarning("Article not found for editing (POST): ID {Id}", id);
                TempData["ErrorMessage"] = "Không tìm thấy bài viết để cập nhật.";
                return RedirectToAction(nameof(Index));
            }

            // Map updated scalar properties and SEO fields
            _mapper.Map(viewModel, article);

            // Handle many-to-many relationships (Tags, Products)
            await UpdateArticleRelationshipsAsync(article, viewModel.SelectedTagIds, viewModel.SelectedProductIds);


            try
            {
                // EF Core will detect changes to article and its relationships
                await _context.SaveChangesAsync();
                _logger.LogInformation("Article updated successfully: {Title} (ID: {Id})", article.Title, article.Id);
                TempData["SuccessMessage"] = $"Đã cập nhật bài viết '{article.Title}' thành công.";
                return RedirectToAction(nameof(Index)); // Redirect to index after update
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating article: ID {Id}, Title: {Title}", id, viewModel.Title);
                // Check for unique slug violation
                if (ex.InnerException?.Message.Contains("idx_articles_slug", StringComparison.OrdinalIgnoreCase) == true ||
                    ex.InnerException?.Message.Contains("articles.slug", StringComparison.OrdinalIgnoreCase) == true)
                {
                    ModelState.AddModelError(nameof(viewModel.Slug), "Slug này đã được sử dụng bởi bài viết khác.");
                }
                else if (ex.InnerException?.Message.Contains("FK_articles_categories", StringComparison.OrdinalIgnoreCase) == true)
                {
                    ModelState.AddModelError(nameof(viewModel.CategoryId), "Danh mục được chọn không tồn tại.");
                }
                else
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật bài viết.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating article: ID {Id}, Title: {Title}", id, viewModel.Title);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật bài viết.");
            }

            // If we are returning to the view, re-populate SelectLists
            await PopulateViewModelSelectListsAsync(viewModel);
        }

        return View(viewModel);
    }


    // POST: Admin/Article/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        // When deleting the parent (Article), EF Core should cascade delete related ArticleTag and ArticleProduct entries
        // due to the configuration (OnDelete(DeleteBehavior.Cascade)).
        // We don't need to explicitly check for linked tags/products before deleting the article itself.
        Article? article = await _context.Set<Article>().FirstOrDefaultAsync(a => a.Id == id);

        if (article == null)
        {
            _logger.LogWarning("Article not found for deletion: ID {Id}", id);
            return Json(new { success = false, message = "Không tìm thấy bài viết." });
        }

        try
        {
            string articleTitle = article.Title;
            _context.Remove(article);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Article deleted successfully: {Title} (ID: {Id})", articleTitle, id);
            return Json(new { success = true, message = $"Xóa bài viết '{articleTitle}' thành công." });
        }
        catch (Exception ex) // Catch generic exception for unexpected issues
        {
            _logger.LogError(ex, "Error deleting article: ID {Id}, Title: {Title}", id, article.Title);
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa bài viết." });
        }
    }


    // --- Helper Methods ---

    private async Task PopulateViewModelSelectListsAsync(ArticleViewModel viewModel)
    {
        viewModel.CategoryOptions = await LoadCategorySelectListAsync(CategoryType.Article, viewModel.CategoryId);
        viewModel.StatusOptions = GetPublishStatusSelectList(viewModel.Status);
        viewModel.TagOptions = await LoadTagSelectListAsync(viewModel.SelectedTagIds); // Pass selected IDs
        viewModel.ProductOptions = await LoadProductSelectListAsync(viewModel.SelectedProductIds); // Pass selected IDs
    }

    private async Task<List<SelectListItem>> LoadCategorySelectListAsync(CategoryType categoryType, int? selectedValue = null)
    {
        var categories = await _context.Set<Category>()
                          .Where(c => c.Type == categoryType && c.IsActive)
                          .OrderBy(c => c.OrderIndex)
                          .ThenBy(c => c.Name)
                          .AsNoTracking()
                          .Select(c => new { c.Id, c.Name })
                          .ToListAsync();

        var items = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Chọn danh mục --", Selected = !selectedValue.HasValue }
        };

        items.AddRange(categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name,
            Selected = selectedValue.HasValue && c.Id == selectedValue.Value
        }));

        return items;
    }

    private async Task<List<SelectListItem>> LoadTagSelectListAsync(List<int>? selectedValues = null)
    {
        var tags = await _context.Set<Tag>()
                          .OrderBy(t => t.Name)
                          .AsNoTracking()
                          .Select(t => new { t.Id, t.Name })
                          .ToListAsync();

        var items = new List<SelectListItem>
        {
            // No default empty option for multi-select usually
        };

        items.AddRange(tags.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Name,
            Selected = selectedValues != null && selectedValues.Contains(t.Id)
        }));

        return items;
    }

    private async Task<List<SelectListItem>> LoadProductSelectListAsync(List<int>? selectedValues = null)
    {
        // Assuming you have a Product entity and DbSet<Product> in DbContext
        var products = await _context.Set<Product>()
                          .OrderBy(p => p.Name)
                          .AsNoTracking()
                          .Select(p => new { p.Id, p.Name })
                          .ToListAsync();

        var items = new List<SelectListItem>
        {
            // No default empty option for multi-select usually
        };

        items.AddRange(products.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = p.Name,
            Selected = selectedValues != null && selectedValues.Contains(p.Id)
        }));

        return items;
    }


    private List<SelectListItem> GetPublishStatusSelectList(PublishStatus? selectedStatus)
    {
        var items = Enum.GetValues(typeof(PublishStatus))
            .Cast<PublishStatus>()
            .Select(t => new SelectListItem
            {
                Value = ((int)t).ToString(),
                Text = t.GetDisplayName(),
                Selected = selectedStatus.HasValue && t == selectedStatus.Value
            })
            .OrderBy(t => t.Text) // Maybe order by enum value instead?
            .ToList();
        return items;
    }

    private List<SelectListItem> GetYesNoSelectList(bool? selectedValue, string allText = "Tất cả")
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = allText, Selected = !selectedValue.HasValue },
            new SelectListItem { Value = "true", Text = "Có", Selected = selectedValue == true },
            new SelectListItem { Value = "false", Text = "Không", Selected = selectedValue == false }
        };
    }

    // Helper to update ArticleTag and ArticleProduct relationships
    private async Task UpdateArticleRelationshipsAsync(Article article, List<int>? selectedTagIds, List<int>? selectedProductIds)
    {
        // Update Tags
        var existingTagIds = article.ArticleTags?.Select(at => at.TagId).ToList() ?? new List<int>();
        var tagIdsToAdd = selectedTagIds?.Except(existingTagIds).ToList() ?? new List<int>();
        var tagIdsToRemove = existingTagIds.Except(selectedTagIds ?? new List<int>()).ToList();

        foreach (var tagId in tagIdsToRemove)
        {
            var articleTag = article.ArticleTags!.First(at => at.TagId == tagId);
            _context.Remove(articleTag); // Remove from context, EF will remove from collection
        }

        foreach (var tagId in tagIdsToAdd)
        {
            // Optional: Validate TagId exists in DB here if not done in Validation
            // if (!_context.Tags.Any(t => t.Id == tagId)) continue; // Skip if tag doesn't exist

            article.ArticleTags ??= new List<ArticleTag>();
            article.ArticleTags.Add(new ArticleTag { ArticleId = article.Id, TagId = tagId });
        }

        // Update Products
        var existingProductIds = article.ArticleProducts?.Select(ap => ap.ProductId).ToList() ?? new List<int>();
        var productIdsToAdd = selectedProductIds?.Except(existingProductIds).ToList() ?? new List<int>();
        var productIdsToRemove = existingProductIds.Except(selectedProductIds ?? new List<int>()).ToList();

        foreach (var productId in productIdsToRemove)
        {
            var articleProduct = article.ArticleProducts!.First(ap => ap.ProductId == productId);
            _context.Remove(articleProduct); // Remove from context, EF will remove from collection
        }

        // Need to determine the correct OrderIndex for added products.
        // For simplicity here, just add them with default OrderIndex=0.
        // A more complex scenario might require managing order via the UI/ViewModel.
        foreach (var productId in productIdsToAdd)
        {
            // Optional: Validate ProductId exists in DB here if not done in Validation
            // if (!_context.Products.Any(p => p.Id == productId)) continue; // Skip if product doesn't exist

            article.ArticleProducts ??= new List<ArticleProduct>();
            article.ArticleProducts.Add(new ArticleProduct { ArticleId = article.Id, ProductId = productId, OrderIndex = 0 });
        }
    }
}