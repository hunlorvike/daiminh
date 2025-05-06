using AutoMapper;
using AutoMapper.QueryableExtensions;
using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Extensions;
using System.Security.Claims;
using web.Areas.Admin.Validators.Article;
using web.Areas.Admin.ViewModels.Article;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class ArticleController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ArticleController> _logger;

    public ArticleController(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<ArticleController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/Article
    public async Task<IActionResult> Index(ArticleFilterViewModel filter, int page = 1, int pageSize = 25)
    {
        filter ??= new ArticleFilterViewModel();
        int pageNumber = page > 0 ? page : 1;
        int currentPageSize = pageSize > 0 ? pageSize : 25;

        IQueryable<Article> query = _context.Set<Article>()
                                             .Include(a => a.Category)
                                             .Include(a => a.ArticleTags)
                                             .Include(a => a.ArticleProducts)
                                             .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(a => a.Title.ToLower().Contains(lowerSearchTerm) ||
                                     (a.Summary != null && a.Summary.ToLower().Contains(lowerSearchTerm)) ||
                                     (a.Content != null && a.Content.ToLower().Contains(lowerSearchTerm)) ||
                                     (a.AuthorName != null && a.AuthorName.ToLower().Contains(lowerSearchTerm)));
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

        query = query.OrderByDescending(a => a.PublishedAt)
                     .ThenByDescending(a => a.CreatedAt);

        var articlesPaged = await query.ProjectTo<ArticleListItemViewModel>(_mapper.ConfigurationProvider)
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
            PublishedAt = DateTime.UtcNow,
            SitemapPriority = 0.5,
            SitemapChangeFrequency = "monthly",
            AuthorName = User.Identity?.Name
        };

        await PopulateViewModelSelectListsAsync(viewModel);

        return View(viewModel);
    }

    // POST: Admin/Article/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ArticleViewModel viewModel)
    {
        var result = await new ArticleViewModelValidator(_context).ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            await PopulateViewModelSelectListsAsync(viewModel);
            return View(viewModel);
        }

        var article = _mapper.Map<Article>(viewModel);
        article.AuthorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        article.AuthorName ??= User.Identity?.Name ?? "Admin";

        UpdateArticleRelationships(article, viewModel.SelectedTagIds, viewModel.SelectedProductIds);

        _context.Add(article);

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã thêm bài viết '{article.Title}' thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi tạo bài viết: {Title}", viewModel.Title);
            ModelState.AddModelError("", "Slug có thể đã tồn tại hoặc dữ liệu không hợp lệ.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tạo bài viết.");
            ModelState.AddModelError("", "Đã xảy ra lỗi hệ thống khi lưu bài viết.");
        }

        await PopulateViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    // GET: Admin/Article/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        Article? article = await _context.Set<Article>()
                                         .Include(a => a.Category)
                                         .Include(a => a.ArticleTags)
                                         .Include(a => a.ArticleProducts)
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null)
        {
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
            _logger.LogWarning("ID không khớp khi chỉnh sửa bài viết. URL: {Id}, ViewModel: {ModelId}", id, viewModel.Id);
            return BadRequest();
        }

        var result = await new ArticleViewModelValidator(_context).ValidateAsync(viewModel);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            await PopulateViewModelSelectListsAsync(viewModel);
            return View(viewModel);
        }

        var article = await _context.Set<Article>()
            .Include(a => a.ArticleTags)
            .Include(a => a.ArticleProducts)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy bài viết.";
            return RedirectToAction(nameof(Index));
        }

        _mapper.Map(viewModel, article);
        UpdateArticleRelationships(article, viewModel.SelectedTagIds, viewModel.SelectedProductIds);

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã cập nhật bài viết '{article.Title}' thành công.";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Lỗi DB khi cập nhật bài viết: ID {Id}", id);
            ModelState.AddModelError("", "Slug có thể đã bị trùng hoặc dữ liệu không hợp lệ.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi hệ thống khi cập nhật bài viết: ID {Id}", id);
            ModelState.AddModelError("", "Đã xảy ra lỗi hệ thống khi cập nhật bài viết.");
        }

        await PopulateViewModelSelectListsAsync(viewModel);
        return View(viewModel);
    }

    // POST: Admin/Article/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        Article? article = await _context.Set<Article>().FirstOrDefaultAsync(a => a.Id == id);

        if (article == null)
        {
            return Json(new { success = false, message = "Không tìm thấy bài viết." });
        }

        try
        {
            string articleTitle = article.Title;
            _context.Remove(article);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = $"Xóa bài viết '{articleTitle}' thành công." });
        }
        catch (Exception)
        {
            return Json(new { success = false, message = "Đã xảy ra lỗi không mong muốn khi xóa bài viết." });
        }
    }



}


public partial class ArticleController
{
    private async Task PopulateViewModelSelectListsAsync(ArticleViewModel viewModel)
    {
        viewModel.CategoryOptions = await LoadCategorySelectListAsync(CategoryType.Article, viewModel.CategoryId);
        viewModel.StatusOptions = GetPublishStatusSelectList(viewModel.Status);
        viewModel.TagOptions = await LoadTagSelectListAsync(viewModel.SelectedTagIds);
        viewModel.ProductOptions = await LoadProductSelectListAsync(viewModel.SelectedProductIds);
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
            new SelectListItem { Value = "", Text = "-- Chọn thẻ --", Selected = selectedValues == null || !selectedValues.Any() }
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
        var products = await _context.Set<Product>()
                          .OrderBy(p => p.Name)
                          .AsNoTracking()
                          .Select(p => new { p.Id, p.Name })
                          .ToListAsync();

        var items = new List<SelectListItem>
    {
        new SelectListItem { Value = "", Text = "-- Chọn sản phẩm --", Selected = selectedValues == null || !selectedValues.Any() }
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
            .OrderBy(t => t.Text)
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

    private List<SelectListItem> GetYesNoSelectList(bool? selectedValue, string allText = "Tất cả")
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = allText, Selected = !selectedValue.HasValue },
            new SelectListItem { Value = "true", Text = "Có", Selected = selectedValue == true },
            new SelectListItem { Value = "false", Text = "Không", Selected = selectedValue == false }
        };
    }

    private void UpdateArticleRelationships(Article article, List<int>? selectedTagIds, List<int>? selectedProductIds)
    {
        var existingTagIds = article.ArticleTags?.Select(at => at.TagId).ToList() ?? new List<int>();
        var tagIdsToAdd = selectedTagIds?.Except(existingTagIds).ToList() ?? new List<int>();
        var tagIdsToRemove = existingTagIds.Except(selectedTagIds ?? new List<int>()).ToList();

        foreach (var tagId in tagIdsToRemove)
        {
            var articleTag = article.ArticleTags!.First(at => at.TagId == tagId);
            _context.Remove(articleTag);
        }

        foreach (var tagId in tagIdsToAdd)
        {
            article.ArticleTags ??= new List<ArticleTag>();
            article.ArticleTags.Add(new ArticleTag { ArticleId = article.Id, TagId = tagId });
        }

        var existingProductIds = article.ArticleProducts?.Select(ap => ap.ProductId).ToList() ?? new List<int>();
        var productIdsToAdd = selectedProductIds?.Except(existingProductIds).ToList() ?? new List<int>();
        var productIdsToRemove = existingProductIds.Except(selectedProductIds ?? new List<int>()).ToList();

        foreach (var productId in productIdsToRemove)
        {
            var articleProduct = article.ArticleProducts!.First(ap => ap.ProductId == productId);
            _context.Remove(articleProduct);
        }

        foreach (var productId in productIdsToAdd)
        {
            article.ArticleProducts ??= new List<ArticleProduct>();
            article.ArticleProducts.Add(new ArticleProduct { ArticleId = article.Id, ProductId = productId });
        }
    }
}