using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TodoTool;

internal class WpfBootstrapper(IServiceProvider provider, IHostApplicationLifetime lifetime) : IHostedService
{
    private readonly IServiceProvider _provider = provider;
    private readonly IHostApplicationLifetime _lifetime = lifetime;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Thread thread = new(RunApplication) { IsBackground = true };

        if (!thread.TrySetApartmentState(ApartmentState.STA))
        {
            thread.SetApartmentState(ApartmentState.Unknown);
            thread.SetApartmentState(ApartmentState.STA);
        }

        thread.Start();

        return Task.CompletedTask;

        //
        void RunApplication()
        {
            Application app = _provider.GetRequiredService<Application>();

            app.Exit += (_, _) => _lifetime.StopApplication();

            app.Run();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
