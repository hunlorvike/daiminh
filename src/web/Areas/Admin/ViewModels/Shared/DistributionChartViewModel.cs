namespace web.Areas.Admin.ViewModels.Shared;

public class DistributionChartViewModel
{
    public List<int> Series { get; init; } = new();
    public List<string> Labels { get; init; } = new();
}
