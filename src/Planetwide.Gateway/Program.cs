using Planetwide.Gateway;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

foreach (var schema in WellKnown.Schemas.All)
{
    builder.Services.AddHttpClient(schema, c =>
    {
        var uri = builder.Configuration[$"Graphql:Endpoint:{schema}"];
        ArgumentNullException.ThrowIfNull(uri, "GraphqlEndpoint");
        c.BaseAddress = new Uri(uri);
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

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGraphQL();

app.Run();
