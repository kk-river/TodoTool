using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;

namespace TodoTool.Modularity;

public interface IModuleCatalog
{
    public void OnInitializing(IHostApplicationBuilder services);
    public void OnInitialized(IServiceProvider provider);

    public void AddModule(IModule module);
    public void AddModule<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TModule>() where TModule : IModule, new();
}
