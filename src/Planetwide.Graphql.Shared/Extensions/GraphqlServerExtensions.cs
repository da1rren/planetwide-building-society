using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            .AddApolloTracing()
            .AddMutationConventions(applyToAllMutations: true);
    }
}