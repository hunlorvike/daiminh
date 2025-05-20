using AutoRegister;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using shared.Extensions;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DashboardService> _logger;

    public DashboardService(ApplicationDbContext context, ILogger<DashboardService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<DashboardViewModel> GetDashboardDataAsync()
    {
        var viewModel = new DashboardViewModel();

        try
        {
            // 1. Total Counts
            viewModel.TotalPublishedArticles = await _context.Set<Article>().CountAsync(a => a.Status == PublishStatus.Published);
            viewModel.TotalDraftArticles = await _context.Set<Article>().CountAsync(a => a.Status == PublishStatus.Draft);
            viewModel.TotalActiveProducts = await _context.Set<Product>().CountAsync(p => p.IsActive && p.Status == PublishStatus.Published);
            viewModel.TotalInactiveProducts = await _context.Set<Product>().CountAsync(p => !p.IsActive || p.Status != PublishStatus.Published);
            viewModel.TotalProductCategories = await _context.Set<domain.Entities.Category>().CountAsync(c => c.Type == CategoryType.Product && c.IsActive);
            viewModel.TotalArticleCategories = await _context.Set<domain.Entities.Category>().CountAsync(c => c.Type == CategoryType.Article && c.IsActive);
            viewModel.TotalFaqCategories = await _context.Set<domain.Entities.Category>().CountAsync(c => c.Type == CategoryType.FAQ && c.IsActive);
            viewModel.TotalProductTags = await _context.Set<Tag>().CountAsync(t => t.Type == TagType.Product);
            viewModel.TotalArticleTags = await _context.Set<Tag>().CountAsync(t => t.Type == TagType.Article);
            viewModel.TotalNewContacts = await _context.Set<domain.Entities.Contact>().CountAsync(c => c.Status == ContactStatus.New);
            viewModel.TotalActiveNewsletters = await _context.Set<domain.Entities.Newsletter>().CountAsync(n => n.IsActive && n.ConfirmedAt != null);
            viewModel.TotalActiveTestimonials = await _context.Set<Testimonial>().CountAsync(t => t.IsActive);
            viewModel.TotalActiveBrands = await _context.Set<domain.Entities.Brand>().CountAsync(b => b.IsActive);
            viewModel.TotalActiveUsers = await _context.Set<User>().CountAsync(u => u.IsActive);
            viewModel.TotalActiveFAQs = await _context.Set<domain.Entities.FAQ>().CountAsync(f => f.IsActive);


            // 2. Article Status Chart (Pie/Donut Chart)
            var articleStatusCounts = await _context.Set<Article>()
                .GroupBy(a => a.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            viewModel.ArticleStatusChart = new ChartData
            {
                Labels = articleStatusCounts.Select(a => a.Status.GetDisplayName()).ToList(),
                SingleSeriesData = articleStatusCounts.Select(a => (decimal)a.Count).ToList()
            };

            var sevenDaysAgo = DateTime.UtcNow.Date.AddDays(-6);
            var recentContactsData = await _context.Set<domain.Entities.Contact>()
                .Where(c => c.CreatedAt >= sevenDaysAgo)
                .GroupBy(c => new { c.CreatedAt.Date, c.Status })
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

            // 4. Products by Category Chart (Top 5) (Bar Chart)
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

            _logger.LogInformation("Dashboard data retrieved successfully.");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving dashboard data.");
            viewModel = new DashboardViewModel
            {
                ArticleStatusChart = new ChartData { Labels = new List<string>(), SingleSeriesData = new List<decimal>() },
                RecentContactsChart = new ChartData { Labels = new List<string>(), Series = new List<ChartSeries>() },
                ProductCategoryChart = new ChartData { Labels = new List<string>(), Series = new List<ChartSeries>() }
            };
        }

        return viewModel;
    }
}