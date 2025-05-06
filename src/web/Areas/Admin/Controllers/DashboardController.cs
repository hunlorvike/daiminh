using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Extensions;
using web.Areas.Admin.ViewModels.Dashboard;
using static web.Areas.Admin.ViewModels.Dashboard.DashboardViewModel;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(ApplicationDbContext context, ILogger<DashboardController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var viewModel = new DashboardViewModel
            {
                TotalPublishedArticles = await _context.Set<Article>().CountAsync(a => a.Status == PublishStatus.Published),
                TotalDraftArticles = await _context.Set<Article>().CountAsync(a => a.Status == PublishStatus.Draft),
                TotalActiveProducts = await _context.Set<Product>().CountAsync(p => p.IsActive && p.Status == PublishStatus.Published),
                TotalInactiveProducts = await _context.Set<Product>().CountAsync(p => !p.IsActive || p.Status != PublishStatus.Published),
                TotalProductCategories = await _context.Set<Category>().CountAsync(c => c.Type == CategoryType.Product && c.IsActive),
                TotalArticleCategories = await _context.Set<Category>().CountAsync(c => c.Type == CategoryType.Article && c.IsActive),
                TotalFaqCategories = await _context.Set<Category>().CountAsync(c => c.Type == CategoryType.FAQ && c.IsActive),
                TotalProductTags = await _context.Set<Tag>().CountAsync(t => t.Type == TagType.Product),
                TotalArticleTags = await _context.Set<Tag>().CountAsync(t => t.Type == TagType.Article),
                TotalNewContacts = await _context.Set<Contact>().CountAsync(c => c.Status == ContactStatus.New),
                TotalActiveNewsletters = await _context.Set<Newsletter>().CountAsync(n => n.IsActive && n.ConfirmedAt != null),
                TotalActiveTestimonials = await _context.Set<Testimonial>().CountAsync(t => t.IsActive),
                TotalActiveBrands = await _context.Set<Brand>().CountAsync(b => b.IsActive),
                TotalActiveUsers = await _context.Set<User>().CountAsync(u => u.IsActive),
                TotalActiveFAQs = await _context.Set<FAQ>().CountAsync(f => f.IsActive),
            };

            // 1. Biểu đồ trạng thái bài viết (Pie/Donut Chart)
            var articleStatusCounts = await _context.Set<Article>()
                .GroupBy(a => a.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            viewModel.ArticleStatusChart = new ChartData // Dùng cho Pie/Donut
            {
                Labels = articleStatusCounts.Select(a => a.Status.GetDisplayName()).ToList(),
                SingleSeriesData = articleStatusCounts.Select(a => (decimal)a.Count).ToList()
            };


            // 2. Biểu đồ liên hệ mới theo 7 ngày gần nhất (Line Chart)
            var sevenDaysAgo = DateTime.UtcNow.Date.AddDays(-6);
            var recentContactsData = await _context.Set<Contact>()
                .Where(c => c.CreatedAt >= sevenDaysAgo) // Lấy tất cả trạng thái trong 7 ngày để có thể so sánh
                .GroupBy(c => new { Date = c.CreatedAt.Date, c.Status })
                .Select(g => new { g.Key.Date, g.Key.Status, Count = g.Count() })
                .OrderBy(x => x.Date)
                .ToListAsync();

            var allDates = Enumerable.Range(0, 7).Select(i => sevenDaysAgo.AddDays(i)).ToList();
            var contactLabels = allDates.Select(d => d.ToString("dd/MM")).ToList();

            var statusSeriesMap = Enum.GetValues(typeof(ContactStatus))
                .Cast<ContactStatus>()
                .ToDictionary(
                    status => status,
                    status => new ChartSeries
                    {
                        Name = status.GetDisplayName(),
                        Data = new List<decimal>()
                    }
                );

            foreach (var date in allDates)
            {
                foreach (var kvp in statusSeriesMap)
                {
                    var status = kvp.Key;
                    var series = kvp.Value;

                    var count = recentContactsData
                        .FirstOrDefault(x => x.Date == date && x.Status == status)
                        ?.Count ?? 0;

                    series.Data.Add(count);
                }
            }

            viewModel.RecentContactsChart = new ChartData
            {
                Labels = contactLabels,
                Series = statusSeriesMap.Values.ToList()
            };

            // 3. Biểu đồ sản phẩm theo danh mục (Top 5) (Bar Chart)
            var productCategoryCounts = await _context.Set<Product>()
                .Where(p => p.IsActive && p.CategoryId != null && p.Category != null)
                .GroupBy(p => new { p.CategoryId, p.Category!.Name })
                .Select(g => new { CategoryName = g.Key.Name, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            viewModel.ProductCategoryChart = new ChartData
            {
                Labels = productCategoryCounts.Select(pc => pc.CategoryName).ToList(),
                Series = new List<ChartSeries>
                 {
                    new ChartSeries { Name = "Số lượng", Data = productCategoryCounts.Select(pc => (decimal)pc.Count).ToList() }
                 }
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tải dữ liệu trang Dashboard.");
            return View(new DashboardViewModel());
        }
    }
}