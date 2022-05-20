using System.Reflection;
using HotChocolate.Types;

namespace Planetwide.Shared.Extensions;

public static class TypeExtensions
{
    public static IEnumerable<Type> DiscoverObjectExtensions(this Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(x => x.CustomAttributes
                .Any(y => y.AttributeType == typeof(ExtendObjectTypeAttribute)));
    }
}