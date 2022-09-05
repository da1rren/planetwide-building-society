using MongoDB.Bson;
using Planetwide.Graphql.Shared.Extensions;
using Planetwide.Prompts.Api.Daemon;
using Planetwide.Prompts.Api.Extensions;
using Planetwide.Prompts.Api.Features;
using Planetwide.Shared;
using Planetwide.Shared.Extensions;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var redisConnectionString = builder.Configuration.GetConnectionString("redis")
    ?? throw new ArgumentNullException("A redis conneciton string must be provided.");

var mongoConnectionString = builder.Configuration.GetConnectionString("mongo")
    ?? throw new ArgumentNullException("A mongo conneciton string must be provided.");

var zipkinConnectionString = builder.Configuration.GetConnectionString("zipkin")
    ?? throw new ArgumentNullException("A zipkin conneciton string must be provided.");

builder.Services
    .RegisterMongoDb(mongoConnectionString)
    .AddHostedService<SeedJob>()
    .AddAuthorization()
    .RegisterRedis(redisConnectionString)
    .RegisterOpenTelemetry("prompts.api", zipkinConnectionString)
    .AddMemoryCache();

builder.Services
    .AddHealthChecks()
    .AddRedis(redisConnectionString)
    .AddMongoDb(mongoConnectionString);

builder.Services
    .AddGraphQLServer()
    .AddPlanetwideDefaults()
    .PublishSchemaDefinition(opt => opt
        .SetName(WellKnown.Schemas.SchemaKey)
        .PublishToRedis(WellKnown.Schemas.Prompts, sp => sp
            .GetRequiredService<ConnectionMultiplexer>()))
    .BindRuntimeType<ObjectId, IdType>()
    .AddTypeConverter<ObjectId, string>(static x => x.ToString())
    .AddTypeConverter<string, ObjectId>(static x => ObjectId.Parse(x))
    .AddQueryType<QueryRoot>()
    .AddMutationType<MutationRoot>()
    .RegisterObjectExtensions(typeof(Program).Assembly);

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