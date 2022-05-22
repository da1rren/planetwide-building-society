using MongoDB.Driver;
using Planetwide.Graphql.Shared.Extensions;
using Planetwide.Shared;
using Planetwide.Shared.Extensions;
using Planetwide.Transactions.Api.Features;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSingleton(sp =>
    {
        var connectionString = builder.Configuration["Database:Mongo"];
        ArgumentNullException.ThrowIfNull(connectionString, "Mongo db connection string");
        return new MongoClient(connectionString);
    })
    .AddAuthorization()
    .RegisterRedis();

builder.Services
    .AddHealthChecks()
    .AddRedis(builder.Configuration["Database:Redis"])
    .AddMongoDb(builder.Configuration["Database:Mongo"]);

builder.Services
    .AddGraphQLServer()
    .InitializeOnStartup()
    .PublishSchemaDefinition(opt => opt
        .SetName(WellKnown.Schemas.SchemaKey)
        .PublishToRedis(WellKnown.Schemas.Transactions, sp => sp
            .GetRequiredService<ConnectionMultiplexer>()))
    .AddMongoDbFiltering()
    .AddMongoDbProjections()
    .AddMongoDbSorting()
    .AddMongoDbPagingProviders()
    .AddQueryType<QueryRoot>()
    .AddMutationType<MutationRoot>()
    .RegisterObjectExtensions(typeof(Program).Assembly);

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
    endpoints.MapDetailedHealthChecks();
});

app.Run();