namespace Planetwide.Gateway.Extensions;

public static class HealthCheckExtensions
{
    // public static IHealthChecksBuilder AddEndpointHttpChecks(this IHealthChecksBuilder builder, IEnumerable<Uri> uris)
    // {
    //     var healthEndpoints = uris.Select(endpoint => new Uri(endpoint, "/health"));
    //     return builder.AddUrlGroup(healthEndpoints);
    // }

    public static IHealthChecksBuilder AddEndpointDnsChecks(this IHealthChecksBuilder builder, IDictionary<string, Uri> schemas)
    {
        foreach (var schema in schemas)
        {
            builder.AddDnsResolveHealthCheck(opt => 
                opt.ResolveHost(schema.Value.DnsSafeHost), $"dns-{schema.Key}");
        }

        return builder;
    }
}