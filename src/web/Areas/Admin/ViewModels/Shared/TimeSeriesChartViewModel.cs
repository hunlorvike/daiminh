namespace web.Areas.Admin.ViewModels.Shared;

public class TimeSeriesChartViewModel
{
    public List<SeriesData> Series { get; init; } = new();
    public List<string> Categories { get; init; } = new();
}
