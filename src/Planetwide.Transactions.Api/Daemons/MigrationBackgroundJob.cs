namespace Planetwide.Transactions.Api.Daemons;

using Features.Transactions;
using MongoDB.Driver;
using Shared;

public class MigrationBackgroundJob : IHostedService
{
    private readonly MongoClient _mongoClient;
    private Task _backgroundTask = null!;

    public MigrationBackgroundJob(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _backgroundTask = MigrateDatabase(cancellationToken);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _backgroundTask;
    }

    private async Task MigrateDatabase(CancellationToken cancellationToken)
    {
        var database = _mongoClient.GetDatabase(WellKnown.Database.MongoDatabase)
            .GetCollection<Transaction>("Transactions");

        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = 1,
                AccountId = 1,
                Amount = -20,
                Reference = "1234"
            },
            new Transaction
            {
                Id = 2,
                AccountId = 1,
                Amount = -12.4,
                Reference = "1234"
            },
            new Transaction
            {
                Id = 3,
                AccountId = 2,
                Amount = -12.4,
                Reference = "1234"
            },
            new Transaction
            {
                Id = 4,
                AccountId = 2,
                Amount = -12.4,
                Reference = "1234"
            }
        };

        foreach (var transaction in transactions)
        {
            await database.InsertOneAsync(transaction, cancellationToken: cancellationToken);
        }
    }
}