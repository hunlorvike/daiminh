using web.Areas.Admin.Services;

namespace web;

public class SeoAnalyticsBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SeoAnalyticsBackgroundService> _logger;

    public SeoAnalyticsBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<SeoAnalyticsBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SEO Analytics Background Service is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("SEO Analytics Background Service is running.");

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var seoService = scope.ServiceProvider.GetRequiredService<ISeoService>();

                    // Lấy dữ liệu từ Google Search Console API
                    // và cập nhật vào cơ sở dữ liệu
                    UpdateSearchConsoleData(seoService);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating SEO analytics data.");
            }

            // Chạy mỗi ngày vào lúc 1 giờ sáng
            var now = DateTime.Now;
            var nextRun = new DateTime(now.Year, now.Month, now.Day, 1, 0, 0).AddDays(1);
            var delay = nextRun - now;

            _logger.LogInformation($"SEO Analytics Background Service is sleeping until {nextRun}.");
            await Task.Delay(delay, stoppingToken);
        }

        _logger.LogInformation("SEO Analytics Background Service is stopping.");
    }

    private void UpdateSearchConsoleData(ISeoService seoService)
    {
        // Triển khai logic để lấy dữ liệu từ Google Search Console API
        // và cập nhật vào cơ sở dữ liệu
    }
}