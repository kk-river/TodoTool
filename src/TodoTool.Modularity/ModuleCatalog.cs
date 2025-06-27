using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;

namespace TodoTool.Modularity;

public class ModuleCatalog : IModuleCatalog
{
    private readonly List<IModule> _modules = [];

    public void OnInitializing(IHostApplicationBuilder builder)
    {
        _modules.ForEach(module => module.OnInitializing(builder));
    }

    public void OnInitialized(IServiceProvider provider)
    {
        _modules.ForEach(module => module.OnInitialized(provider));
    }

    public void AddModule(IModule module)
    {
        _modules.Add(module);
    }

    public void AddModule<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TModule>() where TModule : IModule, new()
    {
        AddModule(new TModule());
    }
}
