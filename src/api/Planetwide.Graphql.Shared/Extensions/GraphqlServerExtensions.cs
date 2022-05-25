using HotChocolate.Execution.Configuration;
using HotChocolate.Execution.Options;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Planetwide.Graphql.Shared.Extensions;

public static class GraphqlServerExtensions
{
    public static IRequestExecutorBuilder AddPlanetwideDefaults(this IRequestExecutorBuilder services)
    {
        return services
            .InitializeOnStartup()
            .AddGlobalObjectIdentification()
            .UseAutomaticPersistedQueryPipeline()
            .AddInMemoryQueryStorage()
            .AddApolloTracing(TracingPreference.Always)
            .AddMutationConventions(applyToAllMutations: true)
            .AddRedisSubscriptions(sp => sp.GetRequiredService<ConnectionMultiplexer>())
            .AddInstrumentation()
            .ModifyRequestOptions(opt =>
            {
                opt.Complexity.ApplyDefaults = true;
                opt.Complexity.DefaultComplexity = 1;
                opt.Complexity.DefaultResolverComplexity = 5;
            });
    }
}