using Microsoft.Extensions.Hosting;

namespace TodoTool.Modularity;

public interface IModule
{
    public void OnInitializing(IHostApplicationBuilder builder);
    public void OnInitialized(IServiceProvider provider);
}
