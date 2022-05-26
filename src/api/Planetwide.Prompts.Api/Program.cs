using MongoDB.Bson;
using Planetwide.Graphql.Shared.Extensions;
using Planetwide.Prompts.Api.Daemon;
using Planetwide.Prompts.Api.Extensions;
using Planetwide.Prompts.Api.Features;
using Planetwide.Shared;
using Planetwide.Shared.Extensions;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .RegisterMongoDb(builder.Configuration["Database:Mongo"])
    .AddHostedService<SeedJob>()
    .AddAuthorization()
    .RegisterRedis()
    .RegisterOpenTelemetry("prompts.api", builder.Configuration["Database:Zipkin"])
    .AddMemoryCache();

builder.Services
    .AddHealthChecks()
    .AddRedis(builder.Configuration["Database:Redis"])
    .AddMongoDb(builder.Configuration["Database:Mongo"]);

builder.Services
    .AddGraphQLServer()
    .AddPlanetwideDefaults()
    .PublishSchemaDefinition(opt => opt
        .SetName(WellKnown.Schemas.SchemaKey)
        .PublishToRedis(WellKnown.Schemas.Prompts, sp => sp
            .GetRequiredService<ConnectionMultiplexer>()))
    .BindRuntimeType<ObjectId, IdType>()
    .AddTypeConverter<ObjectId, string>(x => x.ToString())
    .AddTypeConverter<string, ObjectId>(x => ObjectId.Parse(x))
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