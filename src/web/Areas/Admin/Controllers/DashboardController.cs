using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shared.Constants;
using shared.Enums;
using shared.Models;
using System.Text.Json;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;


namespace web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(AuthenticationSchemes = "AdminScheme", Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(
        IDashboardService dashboardService,
        ILogger<DashboardController> logger)
    {
        _dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> Index()
    {
        DashboardViewModel viewModel;
        try
        {
            viewModel = await _dashboardService.GetDashboardDataAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load dashboard data from service.");
            viewModel = new DashboardViewModel
            {
                ArticleStatusChart = new ChartData { Labels = new List<string>(), SingleSeriesData = new List<decimal>() },
                RecentContactsChart = new ChartData { Labels = new List<string>(), Series = new List<ChartSeries>() },
                ProductCategoryChart = new ChartData { Labels = new List<string>(), Series = new List<ChartSeries>() }
            };

            TempData[TempDataConstants.ToastMessage] = JsonSerializer.Serialize(
                new ToastData("Lỗi", "Không thể tải dữ liệu trang Dashboard. Vui lòng thử lại.", ToastType.Error)
            );
        }

        return View(viewModel);
    }
}
