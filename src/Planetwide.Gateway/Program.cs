using Planetwide.Gateway;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services.AddHttpClient(WellKnown.Schemas.Accounts,
    c => c.BaseAddress = 
        new Uri(builder.Configuration["Graphql:Endpoint:Accounts"]));

builder.Services.AddHttpClient(WellKnown.Schemas.Members,
    c => c.BaseAddress = 
        new Uri(builder.Configuration["Graphql:Endpoint:Members"]));

builder.Services
    .AddGraphQLServer()
    .AddRemoteSchema(WellKnown.Schemas.Accounts)
    .AddRemoteSchema(WellKnown.Schemas.Members)
    .AddTypeExtensionsFromFile("./Stitching.graphql");

// Don't use this in prod.
builder.Services.AddCors(options => 
    options.AddDefaultPolicy(policy => 
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()));

var app = builder.Build();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGraphQL();

app.Run();
