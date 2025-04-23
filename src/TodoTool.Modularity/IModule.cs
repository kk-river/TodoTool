using Microsoft.Extensions.Hosting;

namespace TodoTool.Modularity;

public interface IModule
{
    void Configure(IHostApplicationBuilder builder);
}
