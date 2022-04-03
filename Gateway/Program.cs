using Planetwide.Gateway;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services.AddHttpClient(WellKnown.Schemas.Accounts,
    c => c.BaseAddress = new Uri("https://localhost:7095/graphql/"));

builder.Services.AddHttpClient(WellKnown.Schemas.Members,
    c => c.BaseAddress = new Uri("https://localhost:7222/graphql/"));

builder.Services
    .AddGraphQLServer()
    .AddRemoteSchema(WellKnown.Schemas.Accounts)
    .AddRemoteSchema(WellKnown.Schemas.Members)
    .AddTypeExtensionsFromFile("./Stitching.graphql");

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGraphQL();

app.Run();
