using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;
using Planetwide.Shared;
using Planetwide.Transactions.Api.Features.Transactions;

namespace Planetwide.Transactions.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterMongoDb(this IServiceCollection services, string connectionString)
    {
        return services.AddSingleton(_ =>
            {
                BsonClassMap.RegisterClassMap<BasicTransaction>();
                BsonClassMap.RegisterClassMap<DirectDebitTransaction>();

                BsonClassMap.RegisterClassMap<NetworkMetadata>();
                BsonClassMap.RegisterClassMap<LatencyMetadata>();
                BsonClassMap.RegisterClassMap<RetentionMetadata>();

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
            .AddSingleton<IMongoCollection<TransactionBase>>(sp =>
            {
                var database = sp.GetRequiredService<IMongoDatabase>();
                return database.GetCollection<TransactionBase>("transactions");
            });
    }
}