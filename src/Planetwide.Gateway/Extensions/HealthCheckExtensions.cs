namespace Planetwide.Gateway.Extensions;

public static class HealthCheckExtensions
{
    // public static IHealthChecksBuilder AddEndpointHttpChecks(this IHealthChecksBuilder builder, IEnumerable<Uri> uris)
    // {
    //     var healthEndpoints = uris.Select(endpoint => new Uri(endpoint, "/health"));
    //     return builder.AddUrlGroup(healthEndpoints);
    // }

    public static IHealthChecksBuilder AddEndpointDnsChecks(this IHealthChecksBuilder builder, IEnumerable<Uri> uris)
    {
        var hosts = uris.Select(x => x.DnsSafeHost)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var host in hosts)
        {
            builder.AddDnsResolveHealthCheck(opt => 
                opt.ResolveHost(host), $"dns-{host}");
        }

        return builder;
    }
}