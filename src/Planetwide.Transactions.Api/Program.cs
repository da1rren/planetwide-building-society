using MongoDB.Driver;
using Planetwide.Graphql.Shared.Extensions;
using Planetwide.Shared.Extensions;
using Planetwide.Transactions.Api.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSingleton(sp =>
    {
        var connectionString = builder.Configuration["Database:MongoConnectionString"];
        ArgumentNullException.ThrowIfNull(connectionString, "Mongo db connection string");
        return new MongoClient(connectionString);
    });

builder.Services.AddAuthorization();

builder.Services
    .AddHealthChecks()
    .AddRedis(builder.Configuration["Database:Redis"])
    .AddMongoDb(builder.Configuration["Database:MongoConnectionString"]);

builder.Services
    .AddGraphQLServer()
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