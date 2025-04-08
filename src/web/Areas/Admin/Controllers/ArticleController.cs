using AutoMapper;
using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.Services;
using web.Areas.Admin.ViewModels.Article;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ArticleController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<ArticleViewModel> _validator;
    private readonly ILogger<ArticleController> _logger;
    private readonly IMinioStorageService _minioService;

    public ArticleController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<ArticleViewModel> validator,
        ILogger<ArticleController> logger,
        IMinioStorageService minioService)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
        _minioService = minioService;
    }

    // GET: Admin/Article
    public async Task<IActionResult> Index(string? searchTerm = null, int? categoryId = null, ArticleType? type = null, PublishStatus? status = null)
    {
        ViewData["PageTitle"] = "Quản lý Bài viết";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bài viết", "") };

        var query = _context.Set<Article>()
                            .Include(a => a.ArticleCategories).ThenInclude(ac => ac.Category)
                            // Optional includes for display:
                            // .Include(a => a.Author) // If linking to a User entity
                            .AsQueryable();

        // Filtering
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(a => a.Title.Contains(searchTerm) || (a.Summary != null && a.Summary.Contains(searchTerm)));
        }
        if (categoryId.HasValue)
        {
            query = query.Where(a => a.ArticleCategories.Any(ac => ac.CategoryId == categoryId.Value));
        }
        if (type.HasValue)
        {
            query = query.Where(a => a.Type == type.Value);
        }
        if (status.HasValue)
        {
            query = query.Where(a => a.Status == status.Value);
        }

        var articles = await query.OrderByDescending(a => a.PublishedAt ?? a.CreatedAt).ToListAsync();
        var viewModels = _mapper.Map<List<ArticleListItemViewModel>>(articles);

        // Load filter data
        ViewBag.Categories = await _context.Set<Category>().Where(c => c.Type == CategoryType.Article && c.IsActive).OrderBy(c => c.Name).Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToListAsync();
        ViewBag.Types = Enum.GetValues(typeof(ArticleType)).Cast<ArticleType>().Select(t => new SelectListItem { Value = t.ToString(), Text = GetTypeDisplayName(t) }).ToList();
        ViewBag.Statuses = Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>().Select(s => new SelectListItem { Value = s.ToString(), Text = GetStatusDisplayName(s) }).ToList();

        ViewBag.SearchTerm = searchTerm;
        ViewBag.SelectedCategoryId = categoryId;
        ViewBag.SelectedType = type;
        ViewBag.SelectedStatus = status;

        return View(viewModels);
    }


    // GET: Admin/Article/Create
    public async Task<IActionResult> Create()
    {
        ViewData["PageTitle"] = "Viết Bài mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bài viết", Url.Action(nameof(Index))), ("Viết bài mới", "") };

        var viewModel = new ArticleViewModel
        {
            IsActive = true, // Consider if IsActive is needed for Article
            Status = PublishStatus.Draft,
            PublishedAt = DateTime.UtcNow, // Default to now
            SitemapPriority = 0.7,
            SitemapChangeFrequency = "weekly",
            OgType = "article",
            // TODO: Set default Author from current logged-in user if possible
            // AuthorId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            // AuthorName = User.Identity?.Name
        };

        await LoadDropdownsAsync(viewModel);
        return View(viewModel);
    }

    // POST: Admin/Article/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ArticleViewModel viewModel)
    {
        if (await _context.Set<Article>().AnyAsync(a => a.Slug == viewModel.Slug))
        {
            ModelState.AddModelError(nameof(ArticleViewModel.Slug), "Slug đã tồn tại.");
        }

        // Estimate Reading Time (Simple word count based)
        viewModel.EstimatedReadingMinutes = CalculateReadingTime(viewModel.Content);

        // Set PublishedAt if status is Published and PublishedAt is not set
        if (viewModel.Status == PublishStatus.Published && !viewModel.PublishedAt.HasValue)
        {
            viewModel.PublishedAt = DateTime.UtcNow;
        }
        // Clear PublishedAt if status is not Published
        else if (viewModel.Status != PublishStatus.Published)
        {
            viewModel.PublishedAt = null;
        }

        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid || !ModelState.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            _logger.LogWarning("Article creation validation failed.");
            await LoadDropdownsAsync(viewModel);
            ViewData["PageTitle"] = "Viết Bài mới";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bài viết", Url.Action(nameof(Index))), ("Viết bài mới", "") };
            return View(viewModel);
        }

        var article = _mapper.Map<Article>(viewModel);

        // Handle Relationships
        article.ArticleCategories = viewModel.SelectedCategoryIds.Select(catId => new ArticleCategory { CategoryId = catId }).ToList();
        article.ArticleTags = viewModel.SelectedTagIds.Select(tagId => new ArticleTag { TagId = tagId }).ToList();
        article.ArticleProducts = viewModel.SelectedProductIds.Select((prodId, index) => new ArticleProduct { ProductId = prodId, OrderIndex = index }).ToList(); // Basic ordering

        // Set Author from logged-in user if not provided? (Example)
        // if (string.IsNullOrEmpty(article.AuthorId)) {
        //     article.AuthorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //     article.AuthorName = User.Identity?.Name;
        //     // article.AuthorAvatar = GetUserAvatarPath(article.AuthorId); // Need helper
        // }

        _context.Articles.Add(article);

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Tạo bài viết thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error saving new article.");
            ModelState.AddModelError("", "Không thể lưu bài viết. Vui lòng kiểm tra lại dữ liệu.");
            await LoadDropdownsAsync(viewModel);
            ViewData["PageTitle"] = "Viết Bài mới";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bài viết", Url.Action(nameof(Index))), ("Viết bài mới", "") };
            return View(viewModel);
        }
    }


    // GET: Admin/Article/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var article = await _context.Set<Article>()
            .Include(a => a.ArticleCategories)
            .Include(a => a.ArticleTags)
            .Include(a => a.ArticleProducts).ThenInclude(ap => ap.Product) // Include Product for display name if needed
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null) return NotFound();

        var viewModel = _mapper.Map<ArticleViewModel>(article);
        await LoadDropdownsAsync(viewModel);

        ViewData["PageTitle"] = "Chỉnh sửa Bài viết";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bài viết", Url.Action(nameof(Index))), ("Chỉnh sửa", "") };

        return View(viewModel);
    }


    // POST: Admin/Article/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ArticleViewModel viewModel)
    {
        if (id != viewModel.Id) return BadRequest();

        if (await _context.Set<Article>().AnyAsync(a => a.Slug == viewModel.Slug && a.Id != id))
        {
            ModelState.AddModelError(nameof(ArticleViewModel.Slug), "Slug đã tồn tại.");
        }

        // Estimate Reading Time
        viewModel.EstimatedReadingMinutes = CalculateReadingTime(viewModel.Content);

        // Handle PublishedAt based on Status change
        var originalStatus = await _context.Articles.Where(a => a.Id == id).Select(a => a.Status).FirstOrDefaultAsync();
        if (viewModel.Status == PublishStatus.Published && originalStatus != PublishStatus.Published && !viewModel.PublishedAt.HasValue)
        {
            viewModel.PublishedAt = DateTime.UtcNow; // Set publish date only if transitioning to Published and not already set
        }
        else if (viewModel.Status != PublishStatus.Published)
        {
            viewModel.PublishedAt = null; // Clear publish date if not published
        }

        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid || !ModelState.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            _logger.LogWarning("Article editing validation failed for ID: {ArticleId}", id);
            await LoadDropdownsAsync(viewModel);
            ViewData["PageTitle"] = "Chỉnh sửa Bài viết";
            ViewData["Breadcrumbs"] = new List<(string Text, string Url)> { ("Bài viết", Url.Action(nameof(Index))), ("Chỉnh sửa", "") };
            return View(viewModel);
        }

        var article = await _context.Set<Article>()
            .Include(a => a.ArticleCategories)
            .Include(a => a.ArticleTags)
            .Include(a => a.ArticleProducts)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null) return NotFound();

        // Map scalar properties
        _mapper.Map(viewModel, article);

        // --- Update Relationships ---
        // Categories
        UpdateJunctionTable(article.ArticleCategories, viewModel.SelectedCategoryIds, id, (catId) => new ArticleCategory { ArticleId = id, CategoryId = catId }, fc => fc.CategoryId);
        // Tags
        UpdateJunctionTable(article.ArticleTags, viewModel.SelectedTagIds, id, (tagId) => new ArticleTag { ArticleId = id, TagId = tagId }, ft => ft.TagId);
        // Products
        UpdateJunctionTable(article.ArticleProducts, viewModel.SelectedProductIds, id, (prodId) => new ArticleProduct { ArticleId = id, ProductId = prodId }, fp => fp.ProductId);

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật bài viết thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ArticleExists(id)) return NotFound();
            _logger.LogWarning("Concurrency conflict updating article ID: {ArticleId}", id);
            TempData["ErrorMessage"] = "Lỗi: Xung đột dữ liệu khi cập nhật.";
            await LoadDropdownsAsync(viewModel);
            return View(viewModel);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating article ID: {ArticleId}", id);
            ModelState.AddModelError("", "Không thể lưu bài viết. Vui lòng kiểm tra lại dữ liệu.");
            await LoadDropdownsAsync(viewModel);
            return View(viewModel);
        }
    }


    // POST: Admin/Article/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var article = await _context.Set<Article>()
                                 .Include(a => a.Comments) // Check for comments if needed (cascade delete handles DB)
                                 .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null)
        {
            return Json(new { success = false, message = "Không tìm thấy bài viết." });
        }

        // Store image paths BEFORE deleting the article from DB
        var imagesToDeleteFromMinio = new List<string?> { article.FeaturedImage, article.ThumbnailImage, article.OgImage, article.TwitterImage }
                                        .Where(url => !string.IsNullOrEmpty(url))
                                        .Distinct()
                                        .ToList();


        _context.Articles.Remove(article); // Cascade should handle junction tables

        try
        {
            await _context.SaveChangesAsync();

            // Delete associated images from MinIO AFTER successful DB save
            foreach (var pathToDelete in imagesToDeleteFromMinio)
            {
                if (string.IsNullOrEmpty(pathToDelete)) continue;
                try
                {
                    // Check if it's a full URL or a path before deleting
                    // Simple check: Does it start with http? If yes, maybe don't delete? Or parse path?
                    // Assuming paths are stored:
                    await _minioService.DeleteFileAsync(pathToDelete);
                }
                catch (Exception minioEx)
                {
                    _logger.LogError(minioEx, "Failed to delete image '{ImagePath}' from MinIO after deleting Article ID {ArticleId}.", pathToDelete, id);
                }
            }

            return Json(new { success = true, message = "Xóa bài viết thành công." });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting Article ID {ArticleId}. It might have restrict constraints not handled by cascade.", id);
            return Json(new { success = false, message = "Không thể xóa bài viết. Có thể có lỗi ràng buộc dữ liệu." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Article ID {ArticleId}.", id);
            return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa bài viết." });
        }
    }


    // --- Helper Methods ---
    private async Task LoadDropdownsAsync(ArticleViewModel viewModel)
    {
        viewModel.CategoryList = new SelectList(await _context.Set<Category>().Where(c => c.IsActive && c.Type == CategoryType.Article).OrderBy(c => c.Name).ToListAsync(), "Id", "Name");
        viewModel.TagList = new SelectList(await _context.Set<Tag>().Where(t => t.Type == TagType.Article).OrderBy(t => t.Name).ToListAsync(), "Id", "Name");
        viewModel.ProductList = new SelectList(await _context.Set<Product>().Where(p => p.IsActive && p.Status == PublishStatus.Published).OrderBy(p => p.Name).ToListAsync(), "Id", "Name");
        viewModel.StatusList = new SelectList(Enum.GetValues(typeof(PublishStatus)).Cast<PublishStatus>().Select(e => new { Value = e, Text = GetStatusDisplayName(e) }), "Value", "Text", viewModel.Status);
        viewModel.TypeList = new SelectList(Enum.GetValues(typeof(ArticleType)).Cast<ArticleType>().Select(e => new { Value = e, Text = GetTypeDisplayName(e) }), "Value", "Text", viewModel.Type);
    }

    // Generic helper to update Many-to-Many junction tables
    private void UpdateJunctionTable<TEntity, TKey>(ICollection<TEntity> currentCollection, IEnumerable<TKey> selectedIds, int parentId, Func<TKey, TEntity> createEntity, Func<TEntity, TKey> getKey)
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        var currentIds = currentCollection.Select(getKey).ToList();
        var idsToAdd = selectedIds.Except(currentIds).ToList();
        var entitiesToRemove = currentCollection.Where(e => !selectedIds.Contains(getKey(e))).ToList();

        _context.RemoveRange(entitiesToRemove); // Remove entities from context

        foreach (var idToAdd in idsToAdd)
        {
            currentCollection.Add(createEntity(idToAdd)); // Add new entities to the navigation property
        }
    }


    private string GetStatusDisplayName(PublishStatus status)
    {
        // Consider using Display attribute or resource file
        return status switch
        {
            PublishStatus.Published => "Đã xuất bản",
            PublishStatus.Draft => "Bản nháp",
            PublishStatus.Scheduled => "Đã lên lịch",
            PublishStatus.Archived => "Lưu trữ",
            _ => status.ToString()
        };
    }
    private string GetTypeDisplayName(ArticleType type)
    {
        return type switch
        {
            ArticleType.Knowledge => "Kiến thức",
            ArticleType.News => "Tin tức",
            ArticleType.Promotion => "Khuyến mãi",
            ArticleType.Guide => "Hướng dẫn",
            ArticleType.Review => "Đánh giá",
            _ => type.ToString()
        };
    }
    private int CalculateReadingTime(string content)
    {
        if (string.IsNullOrWhiteSpace(content)) return 0;
        // Simple estimation: 200 words per minute
        var wordCount = content.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
        return (int)Math.Ceiling(wordCount / 200.0);
    }
    private async Task<bool> ArticleExists(int id) => await _context.Set<Article>().AnyAsync(e => e.Id == id);

}