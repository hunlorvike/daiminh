using AutoMapper;
using infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using shared.Enums;
using web.Areas.Admin.Controllers.Shared;
using web.Areas.Client.Models.Content;

namespace web.Areas.Client.Controllers;

[Area("Client")]
public class ContentController : DaiminhController
{
    private readonly ApplicationDbContext _context;

    public ContentController(
        ApplicationDbContext context,
        IMapper mapper,
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        IDistributedCache cache)
        : base(mapper, serviceProvider, configuration, cache)
    {
        _context = context;
    }

    [HttpGet("bai-viet")]
    public async Task<IActionResult> Index(
        string? category = null,
        string? tag = null,
        string? search = null,
        int page = 1,
        int pageSize = 10)
    {
        // Get content type for "bai-viet"
        var contentType = await _context.ContentTypes
            .FirstOrDefaultAsync(ct => ct.Slug == "bai-viet");

        if (contentType == null)
        {
            return NotFound("Không tìm thấy loại nội dung.");
        }

        // Base query for published content of type "bai-viet"
        var query = _context.Contents
            .Where(c => c.ContentTypeId == contentType.Id && c.Status == PublishStatus.Published)
            .Include(c => c.Author)
            .Include(c => c.ContentType)
            .Include(c => c.ContentCategories)
                .ThenInclude(cc => cc.Category)
            .Include(c => c.ContentTags)
                .ThenInclude(ct => ct.Tag)
            .AsQueryable();

        // Apply category filter
        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(c => c.ContentCategories != null &&
                                    c.ContentCategories.Any(cc => cc.Category != null &&
                                                                cc.Category.Slug == category));
        }

        // Apply tag filter
        if (!string.IsNullOrEmpty(tag))
        {
            query = query.Where(c => c.ContentTags != null &&
                                    c.ContentTags.Any(ct => ct.Tag != null &&
                                                          ct.Tag.Slug == tag));
        }

        // Apply search filter
        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            query = query.Where(c => c.Title.ToLower().Contains(search) ||
                                    c.Summary.ToLower().Contains(search) ||
                                    c.ContentBody.ToLower().Contains(search));
        }

        // Count total contents for pagination
        var totalContents = await query.CountAsync();

        // Calculate total pages
        var totalPages = (int)Math.Ceiling(totalContents / (double)pageSize);

        // Ensure page is within valid range
        page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

        // Get paginated contents
        var contents = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Get all categories for content type
        var categories = await _context.Categories
            .Where(cat => cat.EntityType == EntityType.Content)
            .OrderBy(cat => cat.Name)
            .ToListAsync();

        // Create select list for categories
        var categoriesSelectList = categories
            .Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Slug,
                Selected = c.Slug == category
            })
            .ToList();

        // Insert "All Categories" option at the beginning
        categoriesSelectList.Insert(0, new SelectListItem
        {
            Text = "Tất cả chủ đề",
            Value = "",
            Selected = string.IsNullOrEmpty(category)
        });

        // Get popular tags
        var popularTags = await _context.Tags
            .Where(t => t.EntityType == EntityType.Content)
            .OrderBy(t => t.Name)
            .Select(t => t.Slug)
            .Take(20)
            .ToListAsync();

        // Create view model
        var model = new ContentViewModel
        {
            Contents = contents,
            Categories = categories,
            CategoriesSelectList = categoriesSelectList,
            Tags = popularTags,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = totalPages,
            TotalContents = totalContents,
            Search = search ?? "",
            CategorySlug = category ?? "",
            TagSlug = tag ?? "",
            ContentType = contentType
        };

        // Set view title based on filters
        if (!string.IsNullOrEmpty(category))
        {
            var categoryName = categories.FirstOrDefault(c => c.Slug == category)?.Name ?? category;
            ViewBag.Title = $"Bài viết về {categoryName}";
        }
        else if (!string.IsNullOrEmpty(tag))
        {
            ViewBag.Title = $"Bài viết với thẻ #{tag}";
        }
        else if (!string.IsNullOrEmpty(search))
        {
            ViewBag.Title = $"Kết quả tìm kiếm cho '{search}'";
        }
        else
        {
            ViewBag.Title = "Tất cả bài viết";
        }

        return View(model);
    }

    [HttpGet("bai-viet/{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return NotFound();
        }

        var content = await _context.Contents
            .Include(c => c.Author)
            .Include(c => c.ContentType)
            .Include(c => c.ContentCategories)
                .ThenInclude(cc => cc.Category)
            .Include(c => c.ContentTags)
                .ThenInclude(ct => ct.Tag)
            .FirstOrDefaultAsync(c => c.Slug == slug && c.Status == PublishStatus.Published);

        if (content == null)
        {
            return NotFound();
        }

        // Get related contents
        var relatedContents = new List<domain.Entities.Content>();

        if (content.ContentCategories != null && content.ContentCategories.Any())
        {
            var categoryIds = content.ContentCategories.Select(cc => cc.CategoryId).ToList();

            relatedContents = await _context.Contents
                .Where(c => c.Id != content.Id &&
                           c.Status == PublishStatus.Published &&
                           c.ContentCategories != null &&
                           c.ContentCategories.Any(cc => categoryIds.Contains(cc.CategoryId)))
                .OrderByDescending(c => c.CreatedAt)
                .Take(4)
                .ToListAsync();
        }

        var model = new ContentDetailViewModel
        {
            Content = content,
            RelatedContents = relatedContents
        };

        ViewBag.Title = content.Title;

        return View(model);
    }

    [HttpGet("bai-viet/id/{id:int}")]
    public async Task<IActionResult> DetailById(int id)
    {
        var content = await _context.Contents
            .FirstOrDefaultAsync(c => c.Id == id && c.Status == PublishStatus.Published);

        if (content == null)
        {
            return NotFound();
        }

        return RedirectToAction("Detail", new { slug = content.Slug });
    }

    [HttpGet("ve-chung-toi")]
    public IActionResult AboutUs()
    {
        ViewBag.Title = "Về chúng tôi";
        return View();
    }

    [HttpGet("chinh-sach")]
    public async Task<IActionResult> PolicyList(
        string? category = null,
        string? search = null,
        int page = 1,
        int pageSize = 10)
    {
        // Get content type for "chinh-sach"
        var contentType = await _context.ContentTypes
            .FirstOrDefaultAsync(ct => ct.Slug == "chinh-sach");

        if (contentType == null)
        {
            return NotFound("Không tìm thấy loại nội dung.");
        }

        // Base query for published content of type "chinh-sach"
        var query = _context.Contents
            .Where(c => c.ContentTypeId == contentType.Id && c.Status == PublishStatus.Published)
            .Include(c => c.Author)
            .Include(c => c.ContentType)
            .Include(c => c.ContentCategories)
                .ThenInclude(cc => cc.Category)
            .AsQueryable();

        // Apply category filter
        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(c => c.ContentCategories != null &&
                                    c.ContentCategories.Any(cc => cc.Category != null &&
                                                                cc.Category.Slug == category));
        }

        // Apply search filter
        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            query = query.Where(c => c.Title.ToLower().Contains(search) ||
                                    c.Summary.ToLower().Contains(search) ||
                                    c.ContentBody.ToLower().Contains(search));
        }

        // Count total contents for pagination
        var totalContents = await query.CountAsync();

        // Calculate total pages
        var totalPages = (int)Math.Ceiling(totalContents / (double)pageSize);

        // Ensure page is within valid range
        page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

        // Get paginated contents
        var contents = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Get all categories for policy content type
        var categories = await _context.Categories
            .Where(cat => cat.EntityType == EntityType.Content && cat.Slug.StartsWith("chinh-sach-"))
            .OrderBy(cat => cat.Name)
            .ToListAsync();

        // Create select list for categories
        var categoriesSelectList = categories
            .Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Slug,
                Selected = c.Slug == category
            })
            .ToList();

        // Insert "All Categories" option at the beginning
        categoriesSelectList.Insert(0, new SelectListItem
        {
            Text = "Tất cả chính sách",
            Value = "",
            Selected = string.IsNullOrEmpty(category)
        });

        // Create view model
        var model = new ContentViewModel
        {
            Contents = contents,
            Categories = categories,
            CategoriesSelectList = categoriesSelectList,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = totalPages,
            TotalContents = totalContents,
            Search = search ?? "",
            CategorySlug = category ?? "",
            ContentType = contentType
        };

        // Set view title based on filters
        if (!string.IsNullOrEmpty(category))
        {
            var categoryName = categories.FirstOrDefault(c => c.Slug == category)?.Name ?? category;
            ViewBag.Title = $"Chính sách: {categoryName}";
        }
        else if (!string.IsNullOrEmpty(search))
        {
            ViewBag.Title = $"Kết quả tìm kiếm chính sách cho '{search}'";
        }
        else
        {
            ViewBag.Title = "Tất cả chính sách";
        }

        return View("PolicyList", model);
    }

    [HttpGet("chinh-sach/{slug}")]
    public async Task<IActionResult> PolicyDetail(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return NotFound();
        }

        var contentType = await _context.ContentTypes
            .FirstOrDefaultAsync(ct => ct.Slug == "chinh-sach");

        if (contentType == null)
        {
            return NotFound("Không tìm thấy loại nội dung.");
        }

        var content = await _context.Contents
            .Include(c => c.Author)
            .Include(c => c.ContentType)
            .Include(c => c.ContentCategories)
                .ThenInclude(cc => cc.Category)
            .FirstOrDefaultAsync(c => c.Slug == slug && 
                                    c.ContentTypeId == contentType.Id && 
                                    c.Status == PublishStatus.Published);

        if (content == null)
        {
            return NotFound();
        }

        // Get related policies
        var relatedPolicies = new List<domain.Entities.Content>();

        if (content.ContentCategories != null && content.ContentCategories.Any())
        {
            var categoryIds = content.ContentCategories.Select(cc => cc.CategoryId).ToList();

            relatedPolicies = await _context.Contents
                .Where(c => c.Id != content.Id &&
                           c.ContentTypeId == contentType.Id &&
                           c.Status == PublishStatus.Published &&
                           c.ContentCategories != null &&
                           c.ContentCategories.Any(cc => categoryIds.Contains(cc.CategoryId)))
                .OrderByDescending(c => c.CreatedAt)
                .Take(4)
                .ToListAsync();
        }

        var model = new ContentDetailViewModel
        {
            Content = content,
            RelatedContents = relatedPolicies
        };

        ViewBag.Title = content.Title;

        return View("PolicyDetail", model);
    }
}