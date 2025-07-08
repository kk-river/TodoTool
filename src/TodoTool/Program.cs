using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using R3;
using TodoTool.Modularity;
using ZLogger;

namespace TodoTool;

internal class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        IModuleCatalog moduleCatalog = new ModuleCatalog();

        moduleCatalog.AddModule<CoreModule>();
        moduleCatalog.AddModule<SQLiteModule>();

        builder.Services.AddSingleton<Application, App>();

        builder.Services.AddSingleton<MainWindow>();
        builder.Services.AddSingleton<MainWindowViewModel>();

        moduleCatalog.OnInitializing(builder);

        IHost host = builder.Build();

        Application app = host.Services.GetRequiredService<Application>();
        Window window = host.Services.GetRequiredService<MainWindow>();

        ILogger<App> logger = host.Services.GetRequiredService<ILogger<App>>();
        WpfProviderInitializer.SetDefaultObservableSystem(ex => logger.ZLogError($"R3 UnhandledException:{ex}"));

        moduleCatalog.OnInitialized(host.Services);
        host.Start();

        app.Run(window);

        host.StopAsync().GetAwaiter().GetResult();
    }
}
