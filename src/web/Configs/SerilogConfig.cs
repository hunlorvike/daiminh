using Serilog;
using Serilog.Events;

namespace web.Configs;

public static class SerilogConfig
{
    public static void Configure()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .WriteTo.File("Logs/logs-.log", rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Warning,
                fileSizeLimitBytes: 10 * 1024 * 1024,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();
    }
}
