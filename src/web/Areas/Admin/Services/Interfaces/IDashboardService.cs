namespace web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;

public interface IDashboardService
{
    Task<DashboardViewModel> GetDashboardDataAsync();
}
