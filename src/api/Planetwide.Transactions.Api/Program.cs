using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;
using Planetwide.Graphql.Shared.Extensions;
using Planetwide.Shared;
using Planetwide.Shared.Extensions;
using Planetwide.Transactions.Api.Daemons;
using Planetwide.Transactions.Api.Extensions;
using Planetwide.Transactions.Api.Features;
using Planetwide.Transactions.Api.Features.Transactions;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .RegisterMongoDb(builder.Configuration["Database:Mongo"])
    .AddHostedService<SeedJob>()
    .AddAuthorization()
    .AddHttpClient()
    .RegisterRedis()
    .RegisterOpenTelemetry("transactions.api", builder.Configuration["Database:Zipkin"]);

builder.Services
    .AddHealthChecks()
    .AddRedis(builder.Configuration["Database:Redis"])
    .AddMongoDb(builder.Configuration["Database:Mongo"]);

builder.Services
    .AddMemoryCache()
    .AddGraphQLServer()
    .AddPlanetwideDefaults()
    .PublishSchemaDefinition(opt => opt
        .SetName(WellKnown.Schemas.SchemaKey)
        .PublishToRedis(WellKnown.Schemas.Transactions, sp => sp
            .GetRequiredService<ConnectionMultiplexer>()))
    .AddMongoDbFiltering()
    .AddMongoDbProjections()
    .AddMongoDbSorting()
    .AddMongoDbPagingProviders()
    .BindRuntimeType<ObjectId, IdType>()
    .AddTypeConverter<ObjectId, string>(static x => x.ToString())
    .AddTypeConverter<string, ObjectId>(static x => ObjectId.Parse(x))
    .AddQueryType<QueryRoot>()
    .AddMutationType<MutationRoot>()
    .AddSubscriptionType<SubscriptionRoot>()
    .RegisterObjectExtensions(typeof(Program).Assembly)
    .AddType<TransactionBase>()
    .AddType<BasicTransaction>()
    .AddType<DirectDebitTransaction>()
    .AddType<NetworkMetadata>()
    .AddType<LatencyMetadata>()
    .AddType<RetentionMetadata>();

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.UseWebSockets();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
    endpoints.MapDetailedHealthChecks();
});

app.Run();