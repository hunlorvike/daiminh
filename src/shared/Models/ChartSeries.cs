namespace shared.Models;

public class ChartSeries
{
    public string Name { get; set; } = string.Empty;
    public List<decimal> Data { get; set; } = new();
}