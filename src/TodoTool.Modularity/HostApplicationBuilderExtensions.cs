using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;

namespace TodoTool.Modularity;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder LoadModule<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(this IHostApplicationBuilder builder) where T : IModule
    {
        T module = Activator.CreateInstance<T>();
        module.Configure(builder);

        return builder;
    }

    public static IHostApplicationBuilder LoadModule(this IHostApplicationBuilder builder, Type type)
    {
        if (!typeof(IModule).IsAssignableFrom(type))
        {
            throw new ArgumentException($"Type {type.FullName} does not implement {nameof(IModule)}");
        }

        if (type.IsAbstract || type.IsInterface)
        {
            throw new ArgumentException($"Type {type.FullName} is abstract or an interface");
        }

        if (type.GetConstructor(Type.EmptyTypes) == null)
        {
            throw new ArgumentException($"Type {type.FullName} does not have a public parameterless constructor");
        }

        IModule module = (IModule)Activator.CreateInstance(type)!;
        module.Configure(builder);

        return builder;
    }
}
