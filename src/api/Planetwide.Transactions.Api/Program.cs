using MongoDB.Bson;
using Planetwide.Graphql.Shared.Extensions;
using Planetwide.Shared;
using Planetwide.Shared.Extensions;
using Planetwide.Transactions.Api.Daemons;
using Planetwide.Transactions.Api.Extensions;
using Planetwide.Transactions.Api.Features;
using Planetwide.Transactions.Api.Features.Transactions;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var redisConnectionString = builder.Configuration.GetConnectionString("redis")
    ?? throw new ArgumentNullException("A redis conneciton string must be provided.");

var zipkinConnectionString = builder.Configuration.GetConnectionString("zipkin", "api")
    ?? throw new ArgumentNullException("A zipkin conneciton string must be provided.");

var mongoConnectionString = builder.Configuration.GetConnectionString("mongo")
    ?? throw new ArgumentNullException("A mongo conneciton string must be provided.");

builder.Services
    .RegisterMongoDb(mongoConnectionString)
    .AddHostedService<SeedJob>()
    .AddAuthorization()
    .AddHttpClient()
    .RegisterRedis(redisConnectionString)
    .RegisterOpenTelemetry("transactions.api", zipkinConnectionString);

builder.Services
    .AddHealthChecks()
    .AddRedis(redisConnectionString)
    .AddMongoDb(mongoConnectionString);

builder.Services
    .AddMemoryCache()
    .AddGraphQLServer()
    .AddDefaultInstrumentation()
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