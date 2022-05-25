namespace Planetwide.Gateway.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterSchemaHttpClients(this IServiceCollection services,
        IDictionary<string, Uri> schemas)
    {
        foreach (var schema in schemas)
        {
            services.AddHttpClient(schema.Key, c =>
            {
                ArgumentNullException.ThrowIfNull(schema.Value, "GraphqlEndpoint");
                c.BaseAddress = schema.Value;
            });
        }

        return services;
    }
}