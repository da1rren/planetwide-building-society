using HotChocolate.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Planetwide.Challenge.Api;
using Planetwide.Challenge.Api.Infrastructure;
using Planetwide.Challenge.Api.Infrastructure.Directives;
using Planetwide.Challenge.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthorization()
    .AddHttpContextAccessor()
    .AddSingleton<ChallengeService>()
    .AddSingleton<ChallengeMiddleware>()
    .AddHttpResultSerializer<PlanetwideHttpResultSerializer>();

builder.Services.AddAuthentication();

builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Queries>()
    .AddMutationType<Mutations>()
    .AddDirectiveType<ChallengeDirectiveType>()
    .UseInstrumentations()
    .UseExceptions()
    .UseDocumentCache()
    .UseDocumentParser()
    .UseDocumentValidation()
    .UseOperationCache()
    .UseOperationComplexityAnalyzer()
    .UseOperationResolver()
    .UseRequest<ChallengeMiddleware>()
    .UseOperationVariableCoercion()
    .UseOperationExecution();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints
        .MapGraphQL();
});

app.Run();