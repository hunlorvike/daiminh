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
using System.Text.RegularExpressions;
using web.Areas.Admin.Services;
using web.Areas.Admin.ViewModels.Article;
using X.PagedList.EF;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ArticleController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ArticleController> _logger;
    private readonly IMinioStorageService _minioService;

    public ArticleController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ArticleController> logger,
        IMinioStorageService minioService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _minioService = minioService ?? throw new ArgumentNullException(nameof(minioService));
    }

    // GET: Admin/Article
    public async Task<IActionResult> Index(string? searchTerm = null, int? categoryId = null, ArticleType? type = null, PublishStatus? status = null, int page = 1, int pageSize = 15) // Add pagination
    {
        ViewData["Title"] = "Quản lý Bài viết - Hệ thống quản trị";
        ViewData["PageTitle"] = "Danh sách Bài viết";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bài viết", Url.Action(nameof(Index))) };

        int pageNumber = page;
        var query = _context.Set<Article>()
                            .Include(a => a.Category)
                            .AsNoTracking();

        // Filtering
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string lowerSearchTerm = searchTerm.Trim().ToLower();
            query = query.Where(a => a.Title.ToLower().Contains(lowerSearchTerm)
                                  || (a.Summary != null && a.Summary.ToLower().Contains(lowerSearchTerm)));
        }
        if (categoryId.HasValue && categoryId > 0)
        {
            query = query.Where(ac => ac.CategoryId == categoryId.Value);
        }
        if (type.HasValue)
        {
            query = query.Where(a => a.Type == type.Value);
        }
        if (status.HasValue)
        {
            query = query.Where(a => a.Status == status.Value);
        }

        // Sorting & Pagination
        var articlesPaged = await query
            .OrderByDescending(a => a.IsFeatured)
            .ThenByDescending(a => a.PublishedAt ?? a.CreatedAt) // Order by publish date, fallback to creation
            .ProjectTo<ArticleListItemViewModel>(_mapper.ConfigurationProvider) // Project
            .ToPagedListAsync(pageNumber, pageSize); // Paginate

        // Load filter data
        await LoadFilterDropdownsAsync(categoryId, type, status);

        ViewBag.SearchTerm = searchTerm;
        // Selected values loaded into ViewBag by LoadFilterDropdownsAsync

        return View(articlesPaged); // Pass paged list
    }


    // GET: Admin/Article/Create
    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Viết Bài mới - Hệ thống quản trị";
        ViewData["PageTitle"] = "Viết Bài mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bài viết", Url.Action(nameof(Index))), ("Viết bài mới", "") };

        var viewModel = new ArticleViewModel
        {
            //IsActive = true, // Removed, use Status
            Status = PublishStatus.Draft,
            PublishedAt = null, // Let it be null initially
            SitemapPriority = 0.7,
            SitemapChangeFrequency = "weekly",
            OgType = "article",
            SelectedTagIds = new List<int>(),
            SelectedProductIds = new List<int>(),
        };

        // Set default Author from current logged-in user if possible
        // viewModel.AuthorId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get ID if needed
        // viewModel.AuthorName = User.Identity?.Name; // Get display name
        // viewModel.AuthorAvatar = await GetUserAvatarPathAsync(viewModel.AuthorId); // Fetch avatar path

        await LoadRelatedDataForFormAsync(viewModel); // Load dropdowns
        return View(viewModel);
    }

    // POST: Admin/Article/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ArticleViewModel viewModel)
    {
        // --- Manual Checks & Logic ---
        viewModel.EstimatedReadingMinutes = CalculateReadingTime(viewModel.Content);
        HandlePublishDate(viewModel); // Set/clear PublishedAt based on status

        if (ModelState.IsValid)
        {
            var article = _mapper.Map<Article>(viewModel);

            article.ArticleTags = viewModel.SelectedTagIds?
                .Select(tagId => new ArticleTag { TagId = tagId }).ToList() ?? new List<ArticleTag>();
            article.ArticleProducts = viewModel.SelectedProductIds?
                .Select((prodId, index) => new ArticleProduct { ProductId = prodId, OrderIndex = index }).ToList() ?? new List<ArticleProduct>();

            // Set AuthorId if needed (example)
            // if (string.IsNullOrEmpty(article.AuthorId)) {
            //     article.AuthorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // }

            _context.Articles.Add(article);

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Article '{Title}' (ID: {ArticleId}) created successfully by {User}.", article.Title, article.Id, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Tạo bài viết '{Truncate(article.Title)}' thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex) { HandleDbUpdateException(ex, viewModel.Slug); } // Handle unique constraints etc.
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating article '{Title}'.", viewModel.Title);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi tạo bài viết.");
            }
        }
        else
        {
            _logger.LogWarning("Article creation failed for '{Title}'. Model state is invalid.", viewModel.Title);
        }


        // If failed, redisplay form
        await LoadRelatedDataForFormAsync(viewModel); // Reload dropdowns
        ViewData["Title"] = "Viết Bài mới - Hệ thống quản trị";
        ViewData["PageTitle"] = "Viết Bài mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bài viết", Url.Action(nameof(Index))), ("Viết bài mới", "") };
        return View(viewModel);
    }


    // GET: Admin/Article/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var article = await _context.Set<Article>()
            .Include(a => a.Category)
            .Include(a => a.ArticleTags)
            .Include(a => a.ArticleProducts) // No need to ThenInclude Product unless displaying product names here
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null) { _logger.LogWarning("Edit GET: Article ID {ArticleId} not found.", id); return NotFound(); }

        var viewModel = _mapper.Map<ArticleViewModel>(article);
        await LoadRelatedDataForFormAsync(viewModel);

        ViewData["Title"] = "Chỉnh sửa Bài viết - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa Bài viết: {Truncate(article.Title)}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bài viết", Url.Action(nameof(Index))), ($"Chỉnh sửa: {Truncate(article.Title)}", "") };

        return View(viewModel);
    }


    // POST: Admin/Article/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ArticleViewModel viewModel)
    {
        if (id != viewModel.Id) { _logger.LogWarning("Edit POST: ID mismatch. Route ID: {RouteId}, ViewModel ID: {ViewModelId}", id, viewModel.Id); return BadRequest(); }

        // --- Manual Checks & Logic ---
        viewModel.EstimatedReadingMinutes = CalculateReadingTime(viewModel.Content);
        HandlePublishDate(viewModel, await GetOriginalStatusAsync(id)); // Set/clear PublishedAt

        if (ModelState.IsValid)
        {
            var article = await _context.Set<Article>()
                .Include(a => a.Category)
                .Include(a => a.ArticleTags)
                .Include(a => a.ArticleProducts)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null) { _logger.LogWarning("Edit POST: Article ID {ArticleId} not found for update.", id); TempData["error"] = "Không tìm thấy bài viết."; return RedirectToAction(nameof(Index)); }

            // --- Map Scalar Properties ---
            _mapper.Map(viewModel, article);

            try
            {
                // --- Update Relationships ---
                UpdateJunctionTable(article.ArticleTags, viewModel.SelectedTagIds, id, (tagId) => new ArticleTag { ArticleId = id, TagId = tagId }, ft => ft.TagId);
                UpdateJunctionTable(article.ArticleProducts, viewModel.SelectedProductIds, id, (prodId) => new ArticleProduct { ArticleId = id, ProductId = prodId }, fp => fp.ProductId);

                // Set audit fields if needed
                // article.UpdatedBy = User.Identity?.Name;

                await _context.SaveChangesAsync();
                _logger.LogInformation("Article ID {ArticleId} ('{Title}') updated successfully by {User}.", id, article.Title, User.Identity?.Name ?? "Unknown");
                TempData["success"] = $"Cập nhật bài viết '{Truncate(article.Title)}' thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex) { _logger.LogWarning(ex, "Concurrency error updating article ID: {ArticleId}", id); TempData["error"] = "Lỗi: Xung đột dữ liệu."; }
            catch (DbUpdateException ex) { HandleDbUpdateException(ex, viewModel.Slug); } // Handle unique constraints etc.
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating article ID: {ArticleId}", id);
                ModelState.AddModelError("", "Đã xảy ra lỗi không mong muốn khi cập nhật.");
            }
        }
        else
        {
            _logger.LogWarning("Article editing failed for ID: {ArticleId}. Model state is invalid.", id);
        }

        // If failed, redisplay form
        await LoadRelatedDataForFormAsync(viewModel); // Reload dropdowns
        ViewData["Title"] = "Chỉnh sửa Bài viết - Hệ thống quản trị";
        ViewData["PageTitle"] = $"Chỉnh sửa Bài viết: {Truncate(viewModel.Title)}";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bài viết", Url.Action(nameof(Index))), ($"Chỉnh sửa: {Truncate(viewModel.Title)}", "") };
        return View(viewModel);
    }


    // POST: Admin/Article/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var article = await _context.Set<Article>()
                                 // No need to include related data if using Cascade delete
                                 .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null)
        {
            _logger.LogWarning("Delete POST: Article ID {ArticleId} not found.", id);
            return Json(new { success = false, message = "Không tìm thấy bài viết." });
        }

        try
        {
            string title = article.Title;
            // --- Get image paths BEFORE removing entity ---
            var imagesToDelete = new List<string?> { article.FeaturedImage, article.ThumbnailImage, article.OgImage, article.TwitterImage }
                                       .Where(url => !string.IsNullOrEmpty(url) && !url.StartsWith("http")) // Select only non-null paths (not URLs)
                                       .Distinct()
                                       .ToList();

            _context.Articles.Remove(article); // Cascade should handle junction tables
            await _context.SaveChangesAsync(); // Save DB changes

            _logger.LogInformation("Article '{Title}' (ID: {ArticleId}) deleted successfully by {User}.", title, id, User.Identity?.Name ?? "Unknown");

            // --- Delete images from storage AFTER DB delete succeeds ---
            foreach (var path in imagesToDelete)
            {
                if (path == null) continue;
                try
                {
                    await _minioService.DeleteFileAsync(path); // Use injected service
                    _logger.LogInformation("Deleted image '{ImagePath}' from MinIO for deleted Article ID {ArticleId}.", path, id);
                }
                catch (Exception minioEx)
                {
                    _logger.LogError(minioEx, "Failed to delete image '{ImagePath}' from MinIO after deleting Article ID {ArticleId}.", path, id);
                    // Don't fail the entire operation, just log the error
                }
            }

            return Json(new { success = true, message = $"Xóa bài viết '{Truncate(title)}' thành công." });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting Article ID {ArticleId}. Potential RESTRICT constraint violation.", id);
            return Json(new { success = false, message = "Không thể xóa bài viết. Có thể có lỗi ràng buộc dữ liệu." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Article ID {ArticleId}.", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa bài viết." });
        }
    }


    // --- Helper Methods ---

    // Loads dropdowns for Index Filters
    private async Task LoadFilterDropdownsAsync(int? categoryId, ArticleType? type, PublishStatus? status)
    {
        ViewBag.Categories = await _context.Set<Category>().Where(c => c.Type == CategoryType.Article && c.IsActive).OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name, Selected = c.Id == categoryId }).ToListAsync();
        ViewBag.Types = Enum.GetValues(typeof(ArticleType)).Cast<ArticleType>().Select(t => new SelectListItem { Value = t.ToString(), Text = t.GetDisplayName(), Selected = t == type }).ToList();
        ViewBag.Statuses = Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>().Select(s => new SelectListItem { Value = s.ToString(), Text = s.GetDisplayName(), Selected = s == status }).ToList();
        ViewBag.SelectedCategoryId = categoryId;
        ViewBag.SelectedType = type;
        ViewBag.SelectedStatus = status;
    }

    // Loads dropdowns/select lists for Create/Edit Form
    private async Task LoadRelatedDataForFormAsync(ArticleViewModel viewModel)
    {
        viewModel.CategoryList = new SelectList(await _context.Set<Category>().Where(c => c.IsActive && c.Type == CategoryType.Article).OrderBy(c => c.Name).Select(c => new { c.Id, c.Name }).ToListAsync(), "Id", "Name");
        viewModel.TagList = new SelectList(await _context.Set<Tag>().Where(t => t.Type == TagType.Article).OrderBy(t => t.Name).Select(t => new { t.Id, t.Name }).ToListAsync(), "Id", "Name");
        viewModel.ProductList = new SelectList(await _context.Set<Product>().Where(p => p.IsActive && p.Status == PublishStatus.Published).OrderBy(p => p.Name).Select(p => new { p.Id, p.Name }).ToListAsync(), "Id", "Name");
        viewModel.StatusList = new SelectList(Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>().Select(e => new { Value = e, Text = e.GetDisplayName() }), "Value", "Text", viewModel.Status);
        viewModel.TypeList = new SelectList(Enum.GetValues(typeof(ArticleType)).Cast<ArticleType>().Select(e => new { Value = e, Text = e.GetDisplayName() }), "Value", "Text", viewModel.Type);
    }

    // Handles setting/clearing PublishedAt date based on status
    private void HandlePublishDate(ArticleViewModel viewModel, PublishStatus? originalStatus = null)
    {
        if (viewModel.Status == PublishStatus.Published && originalStatus != PublishStatus.Published && !viewModel.PublishedAt.HasValue)
        {
            viewModel.PublishedAt = DateTime.UtcNow;
        }
        else if (viewModel.Status != PublishStatus.Published)
        {
            viewModel.PublishedAt = null; // Clear if not published
        }
        // If status is Published and date is already set, leave it as is (allows manual setting/scheduling)
    }

    // Gets original status for comparison in Edit POST
    private async Task<PublishStatus?> GetOriginalStatusAsync(int articleId)
    {
        return await _context.Articles
                             .Where(a => a.Id == articleId)
                             .Select(a => (PublishStatus?)a.Status) // Cast to nullable
                             .FirstOrDefaultAsync();
    }

    // Generic helper to update Many-to-Many junction tables
    private void UpdateJunctionTable<TEntity, TKey>(ICollection<TEntity> currentCollection, IEnumerable<TKey>? selectedIds, int parentId, Func<TKey, TEntity> createEntity, Func<TEntity, TKey> getKey)
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        selectedIds ??= new List<TKey>(); // Ensure list is not null

        var currentIds = currentCollection.Select(getKey).ToList();
        var idsToAdd = selectedIds.Except(currentIds).ToList();
        var entitiesToRemove = currentCollection.Where(e => !selectedIds.Contains(getKey(e))).ToList();

        if (entitiesToRemove.Any()) _context.RemoveRange(entitiesToRemove);

        if (idsToAdd.Any())
        {
            foreach (var idToAdd in idsToAdd)
            {
                currentCollection.Add(createEntity(idToAdd));
            }
        }
    }

    // Calculates estimated reading time
    private int CalculateReadingTime(string content, int wordsPerMinute = 200)
    {
        if (string.IsNullOrWhiteSpace(content)) return 0;
        // Remove HTML tags before counting words
        var plainText = Regex.Replace(content, "<.*?>", string.Empty);
        var wordCount = plainText.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
        if (wordsPerMinute <= 0) wordsPerMinute = 200; // Avoid division by zero
        return Math.Max(1, (int)Math.Ceiling(wordCount / (double)wordsPerMinute)); // Ensure at least 1 minute
    }

    // Handles DbUpdateException, adding specific ModelErrors
    private void HandleDbUpdateException(DbUpdateException ex, string? slug)
    {
        _logger.LogError(ex, "DbUpdateException occurred.");
        if (ex.InnerException?.Message.Contains("UNIQUE constraint failed: articles.slug", StringComparison.OrdinalIgnoreCase) ?? false)
        {
            ModelState.AddModelError(nameof(ArticleViewModel.Slug), "Slug này đã tồn tại.");
        }
        else
        {
            ModelState.AddModelError("", "Lỗi cơ sở dữ liệu khi lưu.");
        }
    }

    // Truncates string for display
    private string Truncate(string value, int maxLength = 40)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
    }
}
