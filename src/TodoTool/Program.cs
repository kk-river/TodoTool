using System.IO;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoTool.Modularity;

namespace TodoTool;

internal class Program
{
    private static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.LoadModule<SQLiteModule>();

        builder.Services.AddHostedService<WpfBootstrapper>();
        builder.Services.AddSingleton<Application, App>();

        builder.Services.AddSingleton<Window, MainWindow>();
        builder.Services.AddSingleton<MainWindowViewModel>();

        builder.Build().Run();
    }
}
