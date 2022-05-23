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

builder.Services.AddPooledDbContextFactory<AccountContext>(
    options => options.UseSqlite(keepAliveConnection));

builder.Services
    .AddHostedService<MigrationBackgroundJob>()
    .AddAuthorization()
    .RegisterRedis();

builder.Services.AddHealthChecks()
    .AddRedis(builder.Configuration["Database:Redis"]);

builder.Services
    .AddMemoryCache()
    .AddGraphQLServer()
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

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL();
    endpoints.MapDetailedHealthChecks();
});
    
app.Run();
