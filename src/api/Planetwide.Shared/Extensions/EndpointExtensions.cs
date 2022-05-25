namespace Planetwide.Shared.Extensions;

using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;

public static class EndpointExtensions
{
    public static IEndpointConventionBuilder MapDetailedHealthChecks(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        return endpointRouteBuilder.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }
}