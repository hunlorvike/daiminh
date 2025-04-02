using System.Text.RegularExpressions;
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
using web.Areas.Admin.ViewModels.Article;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ArticleController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<ArticleViewModel> _validator;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ArticleController(
        ApplicationDbContext context,
        IMapper mapper,
        IValidator<ArticleViewModel> validator,
        IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET: Admin/Article
    public async Task<IActionResult> Index(ArticleFilterViewModel filter)
    {
        ViewData["PageTitle"] = "Quản lý bài viết";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Bài viết", "")
        };

        var query = _context.Set<Article>()
            .Include(a => a.ArticleCategories)
                .ThenInclude(ac => ac.Category)
            .Include(a => a.ArticleTags)
                .ThenInclude(at => at.Tag)
            .Include(a => a.Comments)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(a => a.Title.Contains(filter.SearchTerm) ||
                                    a.Content.Contains(filter.SearchTerm) ||
                                    a.Summary.Contains(filter.SearchTerm));
        }

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(a => a.ArticleCategories.Any(ac => ac.CategoryId == filter.CategoryId.Value));
        }

        if (filter.TagId.HasValue)
        {
            query = query.Where(a => a.ArticleTags.Any(at => at.TagId == filter.TagId.Value));
        }

        if (filter.Type.HasValue)
        {
            query = query.Where(a => a.Type == filter.Type.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(a => a.Status == filter.Status.Value);
        }

        if (filter.IsFeatured.HasValue)
        {
            query = query.Where(a => a.IsFeatured == filter.IsFeatured.Value);
        }

        if (filter.FromDate.HasValue)
        {
            query = query.Where(a => a.CreatedAt >= filter.FromDate.Value);
        }

        if (filter.ToDate.HasValue)
        {
            var toDate = filter.ToDate.Value.AddDays(1).AddSeconds(-1); // End of the day
            query = query.Where(a => a.CreatedAt <= toDate);
        }

        if (!string.IsNullOrEmpty(filter.AuthorId))
        {
            query = query.Where(a => a.AuthorId == filter.AuthorId);
        }

        var articles = await query
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        var viewModels = _mapper.Map<List<ArticleListItemViewModel>>(articles);

        // Populate categories and tags for each article
        foreach (var viewModel in viewModels)
        {
            var article = articles.First(a => a.Id == viewModel.Id);

            viewModel.Categories = article.ArticleCategories?
                .Select(ac => ac.Category?.Name ?? "")
                .Where(name => !string.IsNullOrEmpty(name))
                .ToList() ?? new List<string>();

            viewModel.Tags = article.ArticleTags?
                .Select(at => at.Tag?.Name ?? "")
                .Where(name => !string.IsNullOrEmpty(name))
                .ToList() ?? new List<string>();

            viewModel.CommentCount = article.Comments?.Count ?? 0;
        }

        // Get categories for dropdown
        ViewBag.Categories = await _context.Set<Category>()
            .OrderBy(c => c.Name)
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = filter.CategoryId.HasValue && c.Id == filter.CategoryId.Value
            })
            .ToListAsync();

        // Get tags for dropdown
        ViewBag.Tags = await _context.Set<Tag>()
            .Where(t => t.Type == TagType.Article)
            .OrderBy(t => t.Name)
            .Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name,
                Selected = filter.TagId.HasValue && t.Id == filter.TagId.Value
            })
            .ToListAsync();

        ViewBag.Filter = filter;

        return View(viewModels);
    }

    // GET: Admin/Article/Create
    public async Task<IActionResult> Create()
    {
        ViewData["PageTitle"] = "Thêm bài viết mới";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Bài viết", "/Admin/Article"),
            ("Thêm mới", "")
        };

        var viewModel = new ArticleViewModel
        {
            Status = PublishStatus.Draft,
            Type = ArticleType.Knowledge,
            AuthorName = User.Identity?.Name ?? "Admin",
            PublishedAt = DateTime.UtcNow
        };

        await PopulateRelatedData(viewModel);

        return View(viewModel);
    }

    // POST: Admin/Article/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ArticleViewModel viewModel)
    {
        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            await PopulateRelatedData(viewModel);
            return View(viewModel);
        }

        // Handle image uploads
        if (viewModel.FeaturedImageFile != null)
        {
            viewModel.FeaturedImage = await UploadImage(viewModel.FeaturedImageFile, "articles");
        }

        if (viewModel.ThumbnailImageFile != null)
        {
            viewModel.ThumbnailImage = await UploadImage(viewModel.ThumbnailImageFile, "articles");
        }

        // Create article
        var article = _mapper.Map<Article>(viewModel);

        // Set current user as author if not specified
        if (string.IsNullOrEmpty(article.AuthorId) && User.Identity?.IsAuthenticated == true)
        {
            article.AuthorId = User.FindFirst("sub")?.Value;
        }

        // Calculate estimated reading time if not provided
        if (article.EstimatedReadingMinutes <= 0)
        {
            article.EstimatedReadingMinutes = CalculateReadingTime(article.Content);
        }

        // Set published date if status is Published
        if (article.Status == PublishStatus.Published && !article.PublishedAt.HasValue)
        {
            article.PublishedAt = DateTime.UtcNow;
        }

        _context.Add(article);
        await _context.SaveChangesAsync();

        // Save categories
        if (viewModel.CategoryIds.Any())
        {
            var articleCategories = viewModel.CategoryIds.Select(categoryId => new ArticleCategory
            {
                ArticleId = article.Id,
                CategoryId = categoryId
            });

            _context.AddRange(articleCategories);
        }

        // Save tags
        if (viewModel.TagIds.Any())
        {
            var articleTags = viewModel.TagIds.Select(tagId => new ArticleTag
            {
                ArticleId = article.Id,
                TagId = tagId
            });

            _context.AddRange(articleTags);
        }

        // Save related products
        if (viewModel.ProductIds.Any())
        {
            var articleProducts = viewModel.ProductIds.Select((productId, index) => new ArticleProduct
            {
                ArticleId = article.Id,
                ProductId = productId,
                OrderIndex = index
            });

            _context.AddRange(articleProducts);
        }

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Thêm bài viết thành công";
        return RedirectToAction(nameof(Edit), new { id = article.Id });
    }

    // GET: Admin/Article/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var article = await _context.Set<Article>()
            .Include(a => a.ArticleCategories)
            .Include(a => a.ArticleTags)
            .Include(a => a.ArticleProducts)
            .Include(a => a.Comments)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null)
        {
            return NotFound();
        }

        ViewData["PageTitle"] = "Chỉnh sửa bài viết";
        ViewData["Breadcrumbs"] = new List<(string Text, string Url)>
        {
            ("Bài viết", "/Admin/Article"),
            ("Chỉnh sửa", "")
        };

        var viewModel = _mapper.Map<ArticleViewModel>(article);

        // Set related data
        viewModel.CategoryIds = article.ArticleCategories?.Select(ac => ac.CategoryId).ToList() ?? new List<int>();
        viewModel.TagIds = article.ArticleTags?.Select(at => at.TagId).ToList() ?? new List<int>();
        viewModel.ProductIds = article.ArticleProducts?.OrderBy(ap => ap.OrderIndex).Select(ap => ap.ProductId).ToList() ?? new List<int>();
        viewModel.CommentCount = article.Comments?.Count ?? 0;

        await PopulateRelatedData(viewModel);

        return View(viewModel);
    }

    // POST: Admin/Article/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ArticleViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var validationResult = await _validator.ValidateAsync(viewModel);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState, string.Empty);
            await PopulateRelatedData(viewModel);
            return View(viewModel);
        }

        try
        {
            var article = await _context.Set<Article>().FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            // Handle image uploads
            if (viewModel.FeaturedImageFile != null)
            {
                viewModel.FeaturedImage = await UploadImage(viewModel.FeaturedImageFile, "articles");
            }

            if (viewModel.ThumbnailImageFile != null)
            {
                viewModel.ThumbnailImage = await UploadImage(viewModel.ThumbnailImageFile, "articles");
            }

            // Update article
            _mapper.Map(viewModel, article);

            // Calculate estimated reading time if not provided
            if (article.EstimatedReadingMinutes <= 0)
            {
                article.EstimatedReadingMinutes = CalculateReadingTime(article.Content);
            }

            // Set published date if status is Published and no date is set
            if (article.Status == PublishStatus.Published && !article.PublishedAt.HasValue)
            {
                article.PublishedAt = DateTime.UtcNow;
            }

            _context.Update(article);

            // Update categories
            var existingCategories = await _context.Set<ArticleCategory>()
                .Where(ac => ac.ArticleId == id)
                .ToListAsync();

            _context.RemoveRange(existingCategories);

            if (viewModel.CategoryIds.Any())
            {
                var articleCategories = viewModel.CategoryIds.Select(categoryId => new ArticleCategory
                {
                    ArticleId = article.Id,
                    CategoryId = categoryId
                });

                _context.AddRange(articleCategories);
            }

            // Update tags
            var existingTags = await _context.Set<ArticleTag>()
                .Where(at => at.ArticleId == id)
                .ToListAsync();

            _context.RemoveRange(existingTags);

            if (viewModel.TagIds.Any())
            {
                var articleTags = viewModel.TagIds.Select(tagId => new ArticleTag
                {
                    ArticleId = article.Id,
                    TagId = tagId
                });

                _context.AddRange(articleTags);
            }

            // Update related products
            var existingProducts = await _context.Set<ArticleProduct>()
                .Where(ap => ap.ArticleId == id)
                .ToListAsync();

            _context.RemoveRange(existingProducts);

            if (viewModel.ProductIds.Any())
            {
                var articleProducts = viewModel.ProductIds.Select((productId, index) => new ArticleProduct
                {
                    ArticleId = article.Id,
                    ProductId = productId,
                    OrderIndex = index
                });

                _context.AddRange(articleProducts);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật bài viết thành công";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ArticleExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToAction(nameof(Edit), new { id });
    }

    // POST: Admin/Article/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var article = await _context.Set<Article>()
            .Include(a => a.Comments)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article == null)
        {
            return Json(new { success = false, message = "Không tìm thấy bài viết" });
        }

        // Check if there are comments
        if (article.Comments != null && article.Comments.Any())
        {
            return Json(new { success = false, message = $"Không thể xóa bài viết này vì có {article.Comments.Count} bình luận. Vui lòng xóa các bình luận trước." });
        }

        try
        {
            // Delete related entities
            var articleCategories = await _context.Set<ArticleCategory>()
                .Where(ac => ac.ArticleId == id)
                .ToListAsync();
            _context.RemoveRange(articleCategories);

            var articleTags = await _context.Set<ArticleTag>()
                .Where(at => at.ArticleId == id)
                .ToListAsync();
            _context.RemoveRange(articleTags);

            var articleProducts = await _context.Set<ArticleProduct>()
                .Where(ap => ap.ArticleId == id)
                .ToListAsync();
            _context.RemoveRange(articleProducts);

            // Delete the article
            _context.Set<Article>().Remove(article);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Xóa bài viết thành công" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Lỗi khi xóa bài viết: {ex.Message}" });
        }
    }

    // POST: Admin/Article/ToggleFeatured/5
    [HttpPost]
    public async Task<IActionResult> ToggleFeatured(int id)
    {
        var article = await _context.Set<Article>().FindAsync(id);

        if (article == null)
        {
            return Json(new { success = false, message = "Không tìm thấy bài viết" });
        }

        article.IsFeatured = !article.IsFeatured;
        await _context.SaveChangesAsync();

        return Json(new { success = true, featured = article.IsFeatured });
    }

    // POST: Admin/Article/UpdateStatus/5
    [HttpPost]
    public async Task<IActionResult> UpdateStatus(int id, PublishStatus status)
    {
        var article = await _context.Set<Article>().FindAsync(id);

        if (article == null)
        {
            return Json(new { success = false, message = "Không tìm thấy bài viết" });
        }

        article.Status = status;

        // Set published date if status is Published and no date is set
        if (status == PublishStatus.Published && !article.PublishedAt.HasValue)
        {
            article.PublishedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return Json(new { success = true, status = status.ToString() });
    }

    // POST: Admin/Article/GenerateSlug
    [HttpPost]
    public IActionResult GenerateSlug(string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            return Json(new { slug = "" });
        }

        var slug = GenerateSlugFromTitle(title);
        return Json(new { slug });
    }

    // Helper methods
    private async Task<bool> ArticleExists(int id)
    {
        return await _context.Set<Article>().AnyAsync(e => e.Id == id);
    }

    private async Task PopulateRelatedData(ArticleViewModel viewModel)
    {
        // Get categories
        var categories = await _context.Set<Category>()
            .OrderBy(c => c.Name)
            .ToListAsync();

        viewModel.AvailableCategories = categories.Select(c => new SelectItemViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Selected = viewModel.CategoryIds.Contains(c.Id)
        }).ToList();

        // Get tags
        var tags = await _context.Set<Tag>()
            .Where(t => t.Type == TagType.Article)
            .OrderBy(t => t.Name)
            .ToListAsync();

        viewModel.AvailableTags = tags.Select(t => new SelectItemViewModel
        {
            Id = t.Id,
            Name = t.Name,
            Selected = viewModel.TagIds.Contains(t.Id)
        }).ToList();

        // Get products
        var products = await _context.Set<Product>()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();

        viewModel.AvailableProducts = products.Select(p => new SelectItemViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Selected = viewModel.ProductIds.Contains(p.Id)
        }).ToList();
    }

    private async Task<string?> UploadImage(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }

        // Create directory if it doesn't exist
        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", folder);
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        // Generate unique filename
        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        // Save file
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return $"/uploads/{folder}/{uniqueFileName}";
    }

    private string GenerateSlugFromTitle(string title)
    {
        // Convert to lowercase
        var slug = title.ToLowerInvariant();

        // Remove diacritics (accents)
        slug = RemoveDiacritics(slug);

        // Replace spaces with hyphens
        slug = Regex.Replace(slug, @"\s+", "-");

        // Remove invalid characters
        slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");

        // Remove multiple hyphens
        slug = Regex.Replace(slug, @"-+", "-");

        // Trim hyphens from start and end
        slug = slug.Trim('-');

        return slug;
    }

    private string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(System.Text.NormalizationForm.FormD);
        var stringBuilder = new System.Text.StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString();
    }

    private int CalculateReadingTime(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return 1;
        }

        // Strip HTML tags
        var plainText = Regex.Replace(content, "<.*?>", string.Empty);

        // Count words (approximately)
        var wordCount = plainText.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;

        // Average reading speed: 200 words per minute
        var minutes = wordCount / 200;

        // Minimum 1 minute
        return Math.Max(1, minutes);
    }
}