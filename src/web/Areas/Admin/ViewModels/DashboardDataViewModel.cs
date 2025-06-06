using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.ViewModels;

public class DashboardDataViewModel
{
    public KpiViewModel NewArticlesKpi { get; set; } = new();
    public KpiViewModel NewProductsKpi { get; set; } = new();
    public KpiViewModel PendingContactsKpi { get; set; } = new();
    public KpiViewModel NewUsersKpi { get; set; } = new();

    public TimeSeriesChartViewModel MainOverviewChart { get; set; } = new();
    public DistributionChartViewModel ContentDistributionChart { get; set; } = new();

    public List<DashboardListItemViewModel> LatestArticles { get; set; } = new();
    public List<DashboardListItemViewModel> TopViewedProducts { get; set; } = new();
    public List<DashboardListItemViewModel> PendingContacts { get; set; } = new();
}
