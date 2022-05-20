using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Planetwide.Members.Api.Daemons;
using Planetwide.Members.Api.Features;
using Planetwide.Members.Api.Infrastructure.Data;
using Planetwide.Shared;
using Planetwide.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// We manage the connection as we are using the in memory mode.  
// The memory is automatically released when we close the last connection
var keepAliveConnection = new SqliteConnection("DataSource=Planetwide;mode=memory;cache=shared");
keepAliveConnection.Open();

builder.Services.AddPooledDbContextFactory<MemberContext>(
    options => options.UseSqlite(keepAliveConnection));

builder.Services.AddHostedService<SeedDataBackgroundJob>();
builder.Services.AddHostedService<MigrationBackgroundJob>();

builder.Services.AddAuthorization();

var graphqlService = builder.Services
    .AddGraphQLServer()
    .AddFiltering()
    .AddProjections()
    .AddSorting()
    .AddQueryableCursorPagingProvider()
    .RegisterDbContext<MemberContext>(DbContextKind.Pooled)
    .AddQueryType<QueryRoot>()
    .AddMutationType<MutationRoot>();

// Using a little reflection we then pull all our type extensions together
// Which lets us break the application into feature folders 
var typeExtensions = typeof(Program).Assembly.DiscoverObjectExtensions();

foreach (var typeExtension in typeExtensions)
{
    graphqlService.AddTypeExtension(typeExtension);
}

var app = builder.Build();

// Normally migrations would be run in a background thread, allowing us to maintain availability
// However as I am creating the database each time this is required
// While our database scheme is altered.
// See MigrationBackgroundJob.cs to see how it can be backgrounded.
var contextFactory = app.Services.GetRequiredService<IDbContextFactory<MemberContext>>();
await using var context = await contextFactory.CreateDbContextAsync();
await context.Database.MigrateAsync();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGraphQL();

app.Run();
