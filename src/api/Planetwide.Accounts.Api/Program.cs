using HotChocolate.Execution.Options;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Planetwide.Accounts.Api.Daemons;
using Planetwide.Accounts.Api.Features;
using Planetwide.Accounts.Api.Infrastructure.Data;
using Planetwide.Graphql.Shared.Extensions;
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

builder.Services.AddPooledDbContextFactory<AccountContext>(
    options => options.UseSqlite(keepAliveConnection));

builder.Services
    .AddHostedService<MigrationBackgroundJob>()
    .AddAuthorization()
    .RegisterRedis(redisConnectionString)
    .RegisterOpenTelemetry("accounts.api", zipkinConnectionString);

builder.Services.AddHealthChecks()
    .AddRedis(redisConnectionString);

builder.Services
    .AddMemoryCache()
    .AddGraphQLServer()
    .AddDefaultInstrumentation()
    .AddPlanetwideDefaults()
    .PublishSchemaDefinition(opt => opt
        .SetName(WellKnown.Schemas.SchemaKey)
        .PublishToRedis(WellKnown.Schemas.Accounts, sp => sp
            .GetRequiredService<ConnectionMultiplexer>()))
    .AddFiltering()
    .AddProjections()
    .AddSorting()
    .AddQueryableCursorPagingProvider()
    .RegisterDbContext<AccountContext>(DbContextKind.Pooled)
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
