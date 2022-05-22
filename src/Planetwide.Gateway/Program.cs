using Planetwide.Gateway.Extensions;
using Planetwide.Shared;
using Planetwide.Shared.Extensions;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var endpoints = WellKnown.Schemas.All.ToDictionary(
    schema => schema, 
    schema => new Uri(builder.Configuration[$"Graphql:Endpoint:{schema}"]));

builder.Services
    .AddAuthorization()
    .RegisterRedis()
    .RegisterSchemaHttpClients(endpoints);

builder.Services.AddHealthChecks()
    .AddEndpointDnsChecks(endpoints)
    .AddRedis(builder.Configuration["Database:Redis"]);

builder.Services
    .AddHealthChecksUI(opt =>
    {
        opt.AddHealthCheckEndpoint("gateway", "/health");

        foreach (var schemas in endpoints)
        {   
            opt.AddHealthCheckEndpoint(schemas.Key, new Uri(schemas.Value, "/health").ToString());
        }
    })
    .AddInMemoryStorage();


var graphqlConfiguration = builder.Services
    .AddGraphQLServer()
    .AddRemoteSchemasFromRedis(WellKnown.Schemas.SchemaKey, sp => 
        sp.GetRequiredService<ConnectionMultiplexer>())
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
    endpoints.MapHealthChecksUI(opt => opt
        .AddCustomStylesheet("wwwroot/healthcheck-ui.css"));
    endpoints.MapDetailedHealthChecks();
});

app.Run();
