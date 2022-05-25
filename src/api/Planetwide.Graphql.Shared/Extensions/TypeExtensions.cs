using System.Reflection;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Planetwide.Graphql.Shared.Extensions;

public static class TypeExtensions
{
    public static IEnumerable<Type> DiscoverObjectExtensions(this Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(x => x.CustomAttributes
                .Any(y => y.AttributeType == typeof(ExtendObjectTypeAttribute)));
    }

    /// <summary>
    /// This method automatically discovers type extensions via reflect and then registers them with
    /// Hotchocolate.  
    /// </summary>
    /// <param name="requestExecutorBuilder">The hotchocolate configuration object</param>
    /// <param name="assembly">The assembly to scan</param>
    /// <returns></returns>
    public static IRequestExecutorBuilder RegisterObjectExtensions(this IRequestExecutorBuilder requestExecutorBuilder,
        Assembly assembly)
    {
        var extensionTypes = DiscoverObjectExtensions(assembly);

        foreach (var type in extensionTypes)
        {
            requestExecutorBuilder.AddTypeExtension(type);
        }

        return requestExecutorBuilder;
    }
}