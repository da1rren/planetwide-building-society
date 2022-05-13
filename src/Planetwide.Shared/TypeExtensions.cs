namespace Planetwide.Shared;

using HotChocolate.Types;
using System.Reflection;

public static class TypeExtensions
{
    public static IEnumerable<Type> DiscoverObjectExtensions(this Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(x => x.CustomAttributes
                .Any(x => x.AttributeType == typeof(ExtendObjectTypeAttribute)));
    }
}