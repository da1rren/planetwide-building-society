using Microsoft.Extensions.Diagnostics.HealthChecks;
using Planetwide.Gateway.Extensions;
using Planetwide.Shared;
using Planetwide.Shared.Extensions;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var redisConnectionString = builder.Configuration.GetConnectionString("redis")
    ?? throw new ArgumentNullException("A redis conneciton string must be provided.");

var zipkinConnectionString = builder.Configuration.GetConnectionString("zipkin", "api")
    ?? throw new ArgumentNullException("A zipkin conneciton string must be provided.");

var endpoints = WellKnown.Schemas.All.ToDictionary(
    schema => schema,
    schema => builder.Configuration.GetServiceUri($"planetwide-{schema}-api") ??
        throw new ArgumentException($"{schema} must provide a uri endpoint."));

builder.Services
    .AddAuthorization()
    .RegisterRedis(redisConnectionString)
    .RegisterOpenTelemetry("Planetwide.Gateway", zipkinConnectionString)
    .RegisterSchemaHttpClients(endpoints);

var gatewayUri = builder.Configuration.GetServiceUri("planetwide-gateway") ??
    throw new ArgumentException($"gateway must provide a uri endpoint.");

var healthCheckBuilder = builder.Services.AddHealthChecks()
    .AddEndpointDnsChecks(endpoints)
    .AddRedis(redisConnectionString);

builder.Services
    .AddHealthChecksUI(opt =>
    {
        opt.AddHealthCheckEndpoint("gateway", new Uri(gatewayUri, "/health").ToString());

        foreach (var schemas in endpoints)
        {

            opt.AddHealthCheckEndpoint(schemas.Key, new Uri(schemas.Value, "/health").ToString());
        }

        opt.SetHeaderText("Planetwide Health Checks");
        opt.MaximumHistoryEntriesPerEndpoint(50);
    })
    .AddInMemoryStorage();


var graphqlConfiguration = builder.Services
    .AddGraphQLServer()
    .AddRemoteSchemasFromRedis(WellKnown.Schemas.SchemaKey, sp =>
        sp.GetRequiredService<ConnectionMultiplexer>())
    .AddTypeExtensionsFromFile("./Stitching.graphql");

foreach (var schemas in WellKnown.Schemas.All)
{
    // HC 13 Features
    // https://github.com/ChilliCream/hotchocolate/issues/5074
    graphqlConfiguration.AddRemoteSchema(schemas /*, capabilities: new EndpointCapabilities
    {
        Subscriptions = SubscriptionSupport.WebSocket
    }*/);
}

// Don't use this in prod.
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()));

var app = builder.Build();

app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.UseWebSockets();

app.UseEndpoints(route =>
{
    route.MapGraphQL();
    route.MapHealthChecksUI(opt => opt
        .AddCustomStylesheet("healthcheck-ui.css")
    );
    route.MapDetailedHealthChecks();
});

app.Run();
