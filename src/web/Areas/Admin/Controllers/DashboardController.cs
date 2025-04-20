using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.ViewModels.Dashboard;
using domain.Entities;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public partial class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(ApplicationDbContext context, ILogger<DashboardController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: Admin/Dashboard
    public async Task<IActionResult> Index()
    {
        try
        {
            var viewModel = new DashboardViewModel
            {
                // Article Counts
                TotalPublishedArticles = await _context.Set<Article>().CountAsync(a => a.Status == PublishStatus.Published),
                TotalDraftArticles = await _context.Set<Article>().CountAsync(a => a.Status == PublishStatus.Draft),

                // Product Counts
                TotalActiveProducts = await _context.Set<Product>().CountAsync(p => p.IsActive && p.Status == PublishStatus.Published),
                TotalInactiveProducts = await _context.Set<Product>().CountAsync(p => !p.IsActive || p.Status != PublishStatus.Published),

                // Category Counts
                TotalProductCategories = await _context.Set<Category>().CountAsync(c => c.Type == CategoryType.Product && c.IsActive),
                TotalArticleCategories = await _context.Set<Category>().CountAsync(c => c.Type == CategoryType.Article && c.IsActive),
                TotalFaqCategories = await _context.Set<Category>().CountAsync(c => c.Type == CategoryType.FAQ && c.IsActive),

                // Tag Counts
                TotalProductTags = await _context.Set<Tag>().CountAsync(t => t.Type == TagType.Product),
                TotalArticleTags = await _context.Set<Tag>().CountAsync(t => t.Type == TagType.Article),

                // Contact Counts
                TotalNewContacts = await _context.Set<Contact>().CountAsync(c => c.Status == ContactStatus.New),

                // Newsletter Counts
                TotalActiveNewsletters = await _context.Set<Newsletter>().CountAsync(n => n.IsActive && n.ConfirmedAt != null),

                // Testimonial Counts
                TotalActiveTestimonials = await _context.Set<Testimonial>().CountAsync(t => t.IsActive),

                // Brand Counts
                TotalActiveBrands = await _context.Set<Brand>().CountAsync(b => b.IsActive),

                // User Counts (Tránh đếm user admin mặc định nếu cần)
                TotalActiveUsers = await _context.Set<User>().CountAsync(u => u.IsActive),

                // FAQ Counts
                TotalActiveFAQs = await _context.Set<FAQ>().CountAsync(f => f.IsActive),

                // Media Counts (Optional)
                TotalMediaFiles = await _context.Set<MediaFile>().CountAsync(),
                TotalMediaFolders = await _context.Set<MediaFolder>().CountAsync(),

                RecentArticles = await LoadRecentArticlesAsync(5),
                RecentContacts = await LoadRecentContactsAsync(5)
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            return View(new DashboardViewModel());
        }
    }
}

public partial class DashboardController
{
    private async Task<List<RecentArticleViewModel>> LoadRecentArticlesAsync(int count)
    {
        return await _context.Set<Article>()
            .OrderByDescending(a => a.CreatedAt)
            .Take(count)
            .Select(a => new RecentArticleViewModel
            {
                Id = a.Id,
                Title = a.Title,
                CreatedAt = a.CreatedAt,
                AuthorName = a.AuthorName
            })
            .AsNoTracking()
            .ToListAsync();
    }

    private async Task<List<RecentContactViewModel>> LoadRecentContactsAsync(int count)
    {
        return await _context.Set<Contact>()
            .Where(c => c.Status == ContactStatus.New)
            .OrderByDescending(c => c.CreatedAt)
            .Take(count)
            .Select(c => new RecentContactViewModel
            {
                Id = c.Id,
                FullName = c.FullName,
                Subject = c.Subject,
                CreatedAt = c.CreatedAt
            })
            .AsNoTracking()
            .ToListAsync();
    }
}