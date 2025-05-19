namespace shared.Models;

public class ChartData
{
    public List<string> Labels { get; set; } = new();
    // Sử dụng List<ChartSeries> cho các biểu đồ có thể có nhiều series
    public List<ChartSeries> Series { get; set; } = new();
    // Hoặc dùng trực tiếp List<decimal> nếu chỉ có 1 series (như Pie/Donut)
    public List<decimal> SingleSeriesData { get; set; } = new();
}