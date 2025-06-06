using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shared.Constants;
using shared.Enums;
using web.Areas.Admin.ViewModels;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Policy = PermissionConstants.AdminAccess)]
public partial class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(ApplicationDbContext context, ILogger<DashboardController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [Authorize(Policy = "Dashboard.View")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboardDataAsync(DateTime? startDate, DateTime? endDate)
    {
        try
        {
            var (fromDate, toDate, previousFromDate, previousToDate) = GetDateRange(startDate, endDate);
            var data = await BuildDashboardDataAsync(fromDate, toDate, previousFromDate, previousToDate);

            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi tải dữ liệu dashboard.");
            return StatusCode(500, "Đã xảy ra lỗi khi tải dữ liệu dashboard.");
        }
    }

    private (DateTime fromDate, DateTime toDate, DateTime previousFromDate, DateTime previousToDate) GetDateRange(DateTime? startDate, DateTime? endDate)
    {
        var toDate = DateTime.SpecifyKind(endDate?.Date.AddDays(1).AddTicks(-1) ?? DateTime.Now.Date.AddDays(1).AddTicks(-1), DateTimeKind.Unspecified);
        var fromDate = DateTime.SpecifyKind(startDate?.Date ?? toDate.AddDays(-7).Date, DateTimeKind.Unspecified);
        var previousToDate = DateTime.SpecifyKind(fromDate.AddTicks(-1), DateTimeKind.Unspecified);
        var previousFromDate = DateTime.SpecifyKind(previousToDate.AddDays(-(toDate - fromDate).TotalDays).Date, DateTimeKind.Unspecified);

        return (fromDate, toDate, previousFromDate, previousToDate);
    }
}

public partial class DashboardController
{
    private async Task<DashboardDataViewModel> BuildDashboardDataAsync(DateTime fromDate, DateTime toDate, DateTime previousFromDate, DateTime previousToDate)
    {
        var data = new DashboardDataViewModel
        {
            NewArticlesKpi = await GetKpiDataAsync(
                _context.Articles.Where(a => a.CreatedAt >= fromDate && a.CreatedAt <= toDate),
                _context.Articles.Where(a => a.CreatedAt >= previousFromDate && a.CreatedAt <= previousToDate)
            ),
            NewProductsKpi = await GetKpiDataAsync(
                _context.Products.Where(p => p.CreatedAt >= fromDate && p.CreatedAt <= toDate),
                _context.Products.Where(p => p.CreatedAt >= previousFromDate && p.CreatedAt <= previousToDate)
            ),
            PendingContactsKpi = await GetKpiDataAsync(
                _context.Contacts.Where(c => c.CreatedAt >= fromDate && c.CreatedAt <= toDate && c.Status == ContactStatus.New),
                _context.Contacts.Where(c => c.CreatedAt >= previousFromDate && c.CreatedAt <= previousToDate && c.Status == ContactStatus.New),
                isDaily: true
            ),
            NewUsersKpi = await GetKpiDataAsync(
                _context.Users.Where(u => u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow),
                _context.Users.Where(u => u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow)
            )
        };

        data.MainOverviewChart = await GetMainOverviewChartAsync(fromDate, toDate);
        data.ContentDistributionChart = await GetContentDistributionChartAsync();
        data.LatestArticles = await GetLatestArticlesAsync();
        data.TopViewedProducts = await GetTopViewedProductsAsync();
        data.PendingContacts = await GetPendingContactsAsync();

        return data;
    }

    private async Task<TimeSeriesChartViewModel> GetMainOverviewChartAsync(DateTime fromDate, DateTime toDate)
    {
        var daysInRange = (toDate.Date - fromDate.Date).Days + 1;
        var dateCategories = Enumerable.Range(0, daysInRange)
            .Select(offset => fromDate.Date.AddDays(offset).ToString("dd/MM"))
            .ToList();

        var chart = new TimeSeriesChartViewModel { Categories = dateCategories };
        var articleViewsSeries = new SeriesData { Name = "Lượt xem bài viết" };
        var productSeries = new SeriesData { Name = "Sản phẩm mới" };
        var contactSeries = new SeriesData { Name = "Liên hệ mới" };

        for (int i = 0; i < daysInRange; i++)
        {
            var currentDate = fromDate.Date.AddDays(i);
            var nextDate = currentDate.AddDays(1);

            articleViewsSeries.Data.Add(await _context.Articles
                .Where(a => a.PublishedAt.HasValue && a.PublishedAt.Value.Date == currentDate)
                .SumAsync(a => a.ViewCount));
            productSeries.Data.Add(await _context.Products
                .CountAsync(p => p.CreatedAt >= currentDate && p.CreatedAt < nextDate));
            contactSeries.Data.Add(await _context.Contacts
                .CountAsync(c => c.CreatedAt >= currentDate && c.CreatedAt < nextDate && c.Status == ContactStatus.New));
        }

        chart.Series.AddRange(new[] { articleViewsSeries, productSeries, contactSeries });
        return chart;
    }

    private async Task<DistributionChartViewModel> GetContentDistributionChartAsync()
    {
        var totalProducts = await _context.Products.CountAsync();
        var totalArticles = await _context.Articles.CountAsync();
        var totalPages = await _context.Pages.CountAsync();
        var totalFaqs = await _context.FAQs.CountAsync();
        var totalContentItems = totalProducts + totalArticles + totalPages + totalFaqs;

        var series = totalContentItems > 0
            ? new List<int>
            {
                (int)Math.Round((double)totalProducts / totalContentItems * 100),
                (int)Math.Round((double)totalArticles / totalContentItems * 100),
                (int)Math.Round((double)totalPages / totalContentItems * 100),
                (int)Math.Round((double)totalFaqs / totalContentItems * 100)
            }
            : new List<int> { 0, 0, 0, 0 };

        return new DistributionChartViewModel
        {
            Series = series,
            Labels = new List<string> { "Sản phẩm", "Bài viết", "Trang tĩnh", "FAQ" }
        };
    }

    private async Task<List<DashboardListItemViewModel>> GetLatestArticlesAsync()
    {
        return await _context.Articles
            .AsNoTracking()
            .OrderByDescending(a => a.PublishedAt ?? a.CreatedAt)
            .Take(5)
            .Select(a => new DashboardListItemViewModel
            {
                Id = a.Id,
                Title = a.Title,
                Subtitle = a.Category != null ? a.Category.Name : "Chưa phân loại",
                DateInfo = (a.PublishedAt ?? a.CreatedAt).ToString("dd/MM/yyyy"),
                Url = Url.Action("Edit", "Article", new { area = "Admin", id = a.Id }) ?? "#"
            })
            .ToListAsync();
    }

    private async Task<List<DashboardListItemViewModel>> GetTopViewedProductsAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .OrderByDescending(p => p.ViewCount)
            .Take(5)
            .Select(p => new DashboardListItemViewModel
            {
                Id = p.Id,
                Title = p.Name,
                Subtitle = p.Category != null ? p.Category.Name : "Chưa phân loại",
                Value = p.ViewCount,
                ValueLabel = "lượt xem",
                Url = Url.Action("Edit", "Product", new { area = "Admin", id = p.Id }) ?? "#"
            })
            .ToListAsync();
    }

    private async Task<List<DashboardListItemViewModel>> GetPendingContactsAsync()
    {
        return await _context.Contacts
            .AsNoTracking()
            .Where(c => c.Status == ContactStatus.New)
            .OrderByDescending(c => c.CreatedAt)
            .Take(5)
            .Select(c => new DashboardListItemViewModel
            {
                Id = c.Id,
                Title = c.FullName,
                Subtitle = c.Subject,
                DateInfo = c.CreatedAt.ToString("dd/MM/yyyy HH:mm"),
                Url = Url.Action("Details", "Contact", new { area = "Admin", id = c.Id }) ?? "#"
            })
            .ToListAsync();
    }

    private async Task<KpiViewModel> GetKpiDataAsync<TEntity>(IQueryable<TEntity> currentQuery, IQueryable<TEntity> previousQuery, bool isDaily = false) where TEntity : class
    {
        var currentCount = await currentQuery.CountAsync();
        var previousCount = await previousQuery.CountAsync();
        var trend = previousCount > 0
            ? Math.Round(((double)(currentCount - previousCount) / previousCount) * 100, 2)
            : currentCount > 0 ? 100 : 0;

        return new KpiViewModel
        {
            Count = currentCount,
            TrendPercentage = trend,
            TrendStatus = trend > 0 ? "up" : trend < 0 ? "down" : "neutral"
        };
    }
}