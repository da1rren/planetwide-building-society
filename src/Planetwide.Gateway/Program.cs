using Planetwide.Gateway;
using Planetwide.Gateway.Extensions;
using Planetwide.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

var endpoints = WellKnown.Schemas.All.Select(schema =>
    new Uri(builder.Configuration[$"Graphql:Endpoint:{schema}"]))
    .ToArray();

builder.Services.AddHealthChecks()
    .AddEndpointDnsChecks(endpoints)
    // .AddEndpointHttpChecks(endpoints)
    .AddRedis(builder.Configuration["Database:Redis"]);

builder.Services
    .AddHealthChecksUI(opt =>
    {
        opt.AddHealthCheckEndpoint("gateway", "/health");

        foreach (var downstreamEndpoint in endpoints.Select(endpoint => new Uri(endpoint, "/health")))
        {
            opt.AddHealthCheckEndpoint(downstreamEndpoint.Host, "/health");
        }
    })
    .AddInMemoryStorage();

foreach (var endpoint in endpoints)
{
    builder.Services.AddHttpClient(endpoint.Host, c =>
    {
        ArgumentNullException.ThrowIfNull(endpoint, "GraphqlEndpoint");
        c.BaseAddress = endpoint;
    });
}

var graphqlConfiguration = builder.Services
    .AddGraphQLServer()
    .AddTypeExtensionsFromFile("./Stitching.graphql");

foreach (var schemas in WellKnown.Schemas.All)
{
    graphqlConfiguration.AddRemoteSchema(schemas);
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

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
    endpoints.MapHealthChecksUI();
    endpoints.MapDetailedHealthChecks();
});

app.Run();
