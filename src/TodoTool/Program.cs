using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TodoTool;

internal class Program
{
    private static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddHostedService<WpfBootstrapper>();
        builder.Services.AddSingleton<Application, App>();
        builder.Services.AddSingleton<Window, MainWindow>();

        builder.Build().Run();
    }
}
