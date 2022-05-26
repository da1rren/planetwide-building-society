using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;
using Planetwide.Prompts.Api.Features.Prompts;
using Planetwide.Shared;

namespace Planetwide.Prompts.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterMongoDb(this IServiceCollection services, string connectionString)
    {
        return services.AddSingleton(_ =>
            {
                ArgumentNullException.ThrowIfNull(connectionString, "Mongo db connection string");

                var clientSettings = MongoClientSettings.FromConnectionString(connectionString);
                clientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());

                return new MongoClient(clientSettings);
            })
            .AddSingleton<IMongoDatabase>(sp =>
            {
                var mongo = sp.GetRequiredService<MongoClient>();
                return mongo.GetDatabase(WellKnown.Database.MongoDatabase);
            })
            .AddSingleton<IMongoCollection<Prompt>>(sp =>
            {
                var database = sp.GetRequiredService<IMongoDatabase>();
                return database.GetCollection<Prompt>("prompts");
            });
    }
}