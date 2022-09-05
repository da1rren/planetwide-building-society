using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Planetwide.Graphql.Shared.Extensions;
using Planetwide.Members.Api.Daemons;
using Planetwide.Members.Api.Features;
using Planetwide.Members.Api.Infrastructure.Data;
using Planetwide.Shared;
using Planetwide.Shared.Extensions;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// We manage the connection as we are using the in memory mode.  
// The memory is automatically released when we close the last connection
var connectionString = builder.Configuration["Database:SqliteConnectionString"];
ArgumentNullException.ThrowIfNull(connectionString);
var keepAliveConnection = new SqliteConnection(connectionString);
keepAliveConnection.Open();

var redisConnectionString = builder.Configuration.GetConnectionString("redis")
    ?? throw new ArgumentNullException("A redis conneciton string must be provided.");

var zipkinConnectionString = builder.Configuration.GetConnectionString("zipkin", "api")
    ?? throw new ArgumentNullException("A zipkin conneciton string must be provided.");

builder.Services.AddPooledDbContextFactory<MemberContext>(
    options => options.UseSqlite(keepAliveConnection));

builder.Services.AddHealthChecks()
    .AddRedis(redisConnectionString);

builder.Services
    .AddAuthorization()
    .AddHostedService<MigrationBackgroundJob>()
    .RegisterRedis(redisConnectionString)
    .RegisterOpenTelemetry("members.api", zipkinConnectionString);

builder.Services
    .AddMemoryCache()
    .AddGraphQLServer()
    .AddDefaultInstrumentation()
    .AddPlanetwideDefaults()
    .PublishSchemaDefinition(opt => opt
        .SetName(WellKnown.Schemas.SchemaKey)
        .PublishToRedis(WellKnown.Schemas.Members, sp => sp
            .GetRequiredService<ConnectionMultiplexer>()))
    .AddFiltering()
    .AddProjections()
    .AddSorting()
    .AddQueryableCursorPagingProvider()
    .RegisterDbContext<MemberContext>(DbContextKind.Pooled)
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
