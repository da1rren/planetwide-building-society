namespace Planetwide.Transactions.Api.Daemons;

using Features.Transactions;
using MongoDB.Driver;
using Shared;

public class MigrationBackgroundJob : IHostedService
{
    private readonly IMongoCollection<Transaction> _mongoCollection;
    private Task _backgroundTask = null!;

    public MigrationBackgroundJob(IMongoCollection<Transaction> mongoCollection)
    {
        _mongoCollection = mongoCollection;
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
        var transactions = new List<Transaction>
        {
            new()
            {
                Id = 1,
                AccountId = 1,
                Amount = -20,
                Reference = "1234"
            },
            new()
            {
                Id = 2,
                AccountId = 1,
                Amount = -12.4m,
                Reference = "1234"
            },
            new()
            {
                Id = 3,
                AccountId = 2,
                Amount = -12.4m,
                Reference = "1234"
            },
            new()
            {
                Id = 4,
                AccountId = 2,
                Amount = 400.4m,
                Reference = "1234"
            }
        };

        foreach (var transaction in transactions)
        {
            await _mongoCollection.InsertOneAsync(
                transaction, cancellationToken: cancellationToken);
        }
    }
}