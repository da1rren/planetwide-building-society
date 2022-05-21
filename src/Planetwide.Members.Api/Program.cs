using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Planetwide.Graphql.Shared.Extensions;
using Planetwide.Members.Api.Daemons;
using Planetwide.Members.Api.Features;
using Planetwide.Members.Api.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// We manage the connection as we are using the in memory mode.  
// The memory is automatically released when we close the last connection
var connectionString = builder.Configuration["Database:SqliteConnectionString"];
ArgumentNullException.ThrowIfNull(connectionString);
var keepAliveConnection = new SqliteConnection(connectionString);
keepAliveConnection.Open();

builder.Services.AddPooledDbContextFactory<MemberContext>(
    options => options.UseSqlite(keepAliveConnection));

builder.Services.AddHostedService<MigrationBackgroundJob>();
builder.Services.AddAuthorization();

builder.Services
    .AddGraphQLServer()
    .AddFiltering()
    .AddProjections()
    .AddSorting()
    .AddQueryableCursorPagingProvider()
    .RegisterDbContext<MemberContext>(DbContextKind.Pooled)
    .AddQueryType<QueryRoot>()
    .AddMutationType<MutationRoot>()
    .RegisterObjectExtensions(typeof(Program).Assembly);

var app = builder.Build();

app.UseAuthorization();

app.MapGraphQL();

app.Run();
