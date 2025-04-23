using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TodoTool.Modularity;
using ZLogger;
using ZLogger.Providers;

namespace TodoTool;

public class CoreModule : IModule
{
    public void Configure(IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton(TimeProvider.System);

        builder.Logging.ClearProviders();
        builder.Logging.AddZLoggerRollingFile(static (ZLoggerRollingFileOptions options, IServiceProvider provider) =>
        {
            options.TimeProvider = provider.GetRequiredService<TimeProvider>();
            options.FilePathSelector = (dt, seq) => Path.Combine("logs", $"DummyGW_{dt.LocalDateTime:yyyyMMdd}_{seq}.log");
            options.RollingInterval = RollingInterval.Day;
            options.RollingSizeKB = 8192;
            options.UsePlainTextFormatter(formatter =>
            {
                formatter.SetPrefixFormatter($"{0:HH:mm:ss.fff} [{1:short}] {2}: ", (in MessageTemplate template, in LogInfo info) => template.Format(info.Timestamp, info.LogLevel, info.Category));
                formatter.SetExceptionFormatter((writer, ex) => Utf8StringInterpolation.Utf8String.Format(writer, $"{ex.Message}"));
            });
        });
    }
}
