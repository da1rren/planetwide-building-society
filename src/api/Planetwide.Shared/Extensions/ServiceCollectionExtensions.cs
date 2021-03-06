using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Planetwide.Shared.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using StackExchange.Redis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterRedis(this IServiceCollection services)
    {
        return services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config["Database:Redis"];
            ArgumentNullException.ThrowIfNull(connectionString);
            return ConnectionMultiplexer.Connect(connectionString);
        });
    }

    public static IServiceCollection RegisterOpenTelemetry(this IServiceCollection services, string serviceName,
        string zipkinEndpoint)
    {
        ArgumentNullException.ThrowIfNull(serviceName);
        ArgumentNullException.ThrowIfNull(zipkinEndpoint);

        return services.AddOpenTelemetryTracing(b =>
        {
            b
                .AddZipkinExporter(opt =>
                {
                    opt.Endpoint = new Uri(zipkinEndpoint);
                })
                .AddSqlClientInstrumentation()
                .AddSource(serviceName)
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(serviceName: serviceName, serviceVersion: "dev"))
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddHotChocolateInstrumentation()
                .AddMongoDBInstrumentation();

            b.Configure((sp, builder) =>
            {
                var multiplexer = sp.GetRequiredService<ConnectionMultiplexer>();
                builder.AddRedisInstrumentation(multiplexer);
            });
        });
    }
}